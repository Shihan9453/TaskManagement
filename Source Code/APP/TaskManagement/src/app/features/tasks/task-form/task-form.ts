import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TaskInterface } from '../task-interface';
import { TaskService } from '../task-service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../../shared/alert/alert-service';
import { UserInterface } from '../user-interface';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-task-form',
  standalone: false,
  templateUrl: './task-form.html',
  styleUrl: './task-form.css',
})

export class TaskForm implements OnInit 
{
  isEditMode = false;
  today!: string;
  validationErrors: { [key: string]: string } = {};

  tasks: TaskInterface = 
  {
    id: 0,
    title: '',
    description: '',
    status: '',
    priorityLevel: '',
    assignedPerson: '',
    assignedPersonId: 0,
    dueDate: '',
    completedDate: '',
    remarks: ''
  };

  usersList: UserInterface[] = [];
  statusList: string[] = [];
  priorityLevelsList: string[] = [];

  constructor(private taskService: TaskService, private router: Router, private route: ActivatedRoute, private alertService: AlertService, private cdr: ChangeDetectorRef) 
  { 
  }

  ngOnInit(): void 
  {
    const nowDate = new Date();
    this.today = nowDate.toISOString().split('T')[0];

    forkJoin({
      users: this.taskService.GetUsers(),
      statuses: this.taskService.GetStatuses(),
      priorities: this.taskService.GetPriorityLevels()
    }).subscribe({
    next: (res) => 
    {
      this.usersList = res.users.data;
      this.statusList = res.statuses.data;
      this.priorityLevelsList = res.priorities.data;

      this.tasks.assignedPersonId = this.usersList[0]?.id || 0;
      this.tasks.status = this.statusList[0] || '';
      this.tasks.priorityLevel = this.priorityLevelsList[0] || '';

      this.taskService.selectedTask$.subscribe(task => 
      {
        if (task) 
        {
          this.isEditMode = true;
          this.tasks = { ...task };
          this.tasks.dueDate = this.formatDate(this.tasks.dueDate);

          if (this.tasks.completedDate) 
          {
            this.tasks.completedDate = this.formatDate(this.tasks.completedDate);
          }
        } 
      });

      this.cdr.detectChanges();
    }
    });
  }
  
  formatDate(date: string | Date): string 
  {
    const d = new Date(date);
    if (isNaN(d.getTime())) 
    {
      this.alertService.showError('Invalid date: ' + date);
      return '';
    }

    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;

  }

  SaveTask() 
  {
    this.taskService.SaveTask(this.tasks).subscribe({
    next: (data) => 
    {
      this.alertService.showSuccess(data.details);
      this.taskService.triggerRefresh();
      this.ClearUI();
      this.cdr.detectChanges();  
    },
    error:(err)=>
    {      
      if (err?.error?.errors) 
      {
        const validationErrors = err.error.errors;
        const errorMessages = Object.keys(validationErrors).map(key => validationErrors[key].join(' ')).join('\n');
        
        this.alertService.showError(errorMessages);
        this.cdr.detectChanges(); 
      } 
      else if (err?.error?.details) 
      {
        this.alertService.showError(err.error.details);
        this.cdr.detectChanges(); 
      } 
    }      
    });
  }

  UpdateTask() 
  {
    this.taskService.UpdateTask(this.tasks).subscribe({
    next: (data) => 
    {
      this.alertService.showSuccess(data.details);
      this.taskService.triggerRefresh();
      this.ClearUI();
      this.cdr.detectChanges();  
    },
    error:(err)=>
    {      
      if (err?.error?.errors) 
      {
        const validationErrors = err.error.errors;
        const errorMessages = Object.keys(validationErrors).map(key => validationErrors[key].join(' ')).join('\n');

        this.alertService.showError(errorMessages);
        this.cdr.detectChanges(); 
      } 
      else if (err?.error?.details) 
      {
        this.alertService.showError(err.error.details);
        this.cdr.detectChanges(); 
      } 
    }
    });
  }

  Clear() 
  {
     this.ClearUI();
     this.cdr.detectChanges();
  }

  ClearUI() 
  {
    this.tasks = 
    {
      id: 0,
      title: '',
      description: '',
      status: this.statusList.length ? this.statusList[0] : '',
      priorityLevel: this.priorityLevelsList.length ? this.priorityLevelsList[0] : '',
      assignedPerson: '',
      assignedPersonId: this.usersList.length ? this.usersList[0].id : 0,
      dueDate: '',
      completedDate: '',
      remarks: ''
    };

    this.validationErrors = {};
    this.isEditMode = false;
  }
  
  ClearErrors(field: string): void 
  {
    if (this.validationErrors[field]) 
    {
      delete this.validationErrors[field];
    }
    this.cdr.detectChanges();
  }
 
  onStatusChange(status: string, control: any) 
  {
   if (status !== 'Completed') 
   {
    this.tasks.completedDate = undefined;   
    control.reset();                        
   }
  }  

}