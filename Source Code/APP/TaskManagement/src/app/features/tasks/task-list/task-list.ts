import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TaskInterface } from '../task-interface';
import { TaskService } from '../task-service';
import { AlertService } from '../../../shared/alert/alert-service';
import { ActivatedRoute, Router } from '@angular/router';
declare var bootstrap: any;

@Component({
  selector: 'app-task-list',
  standalone: false,
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})

export class TaskList implements OnInit   
{

  tasks:TaskInterface[]=[];
  taskIdToDelete: number | null = null;
  page = 1;
  modalRef: any;

  filteredTasks: TaskInterface[] = [];
  filterText = '';
  filterStatus = '';
  filterPriority = '';
  sortColumn = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  constructor(private taskService: TaskService, private alertService: AlertService, private cdr: ChangeDetectorRef, private router: Router, private route: ActivatedRoute)
  {
  }
  
  ngOnInit(): void 
  {
    this.LoadTasks();
    this.taskService.refreshList$.subscribe(() => 
    {
      this.LoadTasks();
    });
  }

  LoadTasks() 
  {
    this.taskService.GetTasks().subscribe((response) => 
    {
      this.tasks = [...response.data];
      this.applyFilters();
      this.cdr.detectChanges();
    });
  } 

  DeleteTask(id: number) 
  {
    this.taskIdToDelete = id;
    const mdlId = document.getElementById('mdlDelConfirm');
    if (mdlId) 
    {
      this.modalRef = new bootstrap.Modal(mdlId, 
      {
          backdrop: 'static',  
          keyboard: false      
      });

      mdlId.addEventListener('hidden.bs.modal', () => 
      {
          this.modalRef?.dispose();
          this.modalRef = null;
      }, { once: true });        

      this.modalRef.show();     
    }
  }

  DeleteConfirmation() 
  { 
    if (this.taskIdToDelete !== null) 
    {
      this.taskService.DeleteTask(this.taskIdToDelete).subscribe({
      next: (data) =>
      {
          this.tasks = this.tasks.filter(itm => itm.id !== this.taskIdToDelete); 
          this.taskIdToDelete = null;
          if (this.modalRef) 
          {
            this.modalRef.hide();
          }

          this.alertService.showSuccess(data.details);
          this.taskService.triggerRefresh();
          this.cdr.markForCheck();
      },
      error: (err) => 
      {
          if (err?.error?.errors) 
          {
           const validationErrors = err.error.errors;
           const errorMessages = Object.keys(validationErrors).map(key => validationErrors[key].join(' ')).join('\n');

           this.alertService.showError(errorMessages);
           this.cdr.markForCheck();
          } 
          else if (err?.error?.details) 
          {
           this.alertService.showError(err.error.details);
           this.cdr.markForCheck();
          } 
      }
      });
    }
  }
  
  MarkCompleted(id: number) 
  {
    const task = this.tasks.find(t => t.id === id);
    if (!task) return;
    const today = new Date().toISOString().split('T')[0];
    const updatedTask = {...task, status: 'Completed', completedDate: today};
    this.taskService.UpdateTask(updatedTask).subscribe({
    next: (response) => 
    {
        this.alertService.showSuccess(response.details);
        this.taskService.triggerRefresh();
        this.cdr.markForCheck();
    },
    error:(err)=>
    {      
      if (err?.error?.errors) 
      {
        const validationErrors = err.error.errors;
        const errorMessages = Object.keys(validationErrors).map(key => validationErrors[key].join(' ')).join('\n');

        this.alertService.showError(errorMessages);
        this.cdr.markForCheck();
      } 
      else if (err?.error?.details) 
      {
        this.alertService.showError(err.error.details);
        this.cdr.markForCheck();
      } 
    }
    });
  }

  CloseModal() 
  {
    const mdlId = document.getElementById('mdlDelConfirm');
    if (mdlId) 
    {      
      const modal = bootstrap.Modal.getInstance(mdlId);
      if (modal) 
      {
        modal.hide();
      }
    }
  }

  EditTask(task: TaskInterface) 
  {
    this.taskService.setSelectedTask(task);
  }

  applyFilters() 
  {
    let data = [...this.tasks];
    if (this.filterText) 
    {
      data = data.filter(t => t.title?.toLowerCase().includes(this.filterText.toLowerCase()));
    }
    if (this.filterStatus) 
    {
      data = data.filter(t => t.status === this.filterStatus);
    }
    if (this.filterPriority) 
    {
      data = data.filter(t => t.priorityLevel === this.filterPriority);
    }

    this.filteredTasks = data;
    if (this.sortColumn) 
    {
      this.sort(this.sortColumn);
    }
  }

  sort(column: string) 
  {
    if (this.sortColumn === column) 
    {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } 
    else 
    {
      this.sortDirection = 'asc';
    }

    this.sortColumn = column;
    this.filteredTasks = [...this.filteredTasks].sort((a: any, b: any) => 
    {
      let valA = a[column];
      let valB = b[column];

      if (valA == null) valA = '';
      if (valB == null) valB = '';

      if (column.toLowerCase().includes('date')) 
      {
        valA = new Date(valA).getTime();
        valB = new Date(valB).getTime();
      }

      if (typeof valA === 'string') 
      {
        valA = valA.toLowerCase();
        valB = valB.toLowerCase();
      }

      if (valA < valB) return this.sortDirection === 'asc' ? -1 : 1;
      if (valA > valB) return this.sortDirection === 'asc' ? 1 : -1;
      return 0;
    });
  }

  getSortIcon(column: string): string 
  {
    if (this.sortColumn !== column) 
    {
      return 'fa fa-sort text-muted'; 
    }
    return this.sortDirection === 'asc' ? 'fa fa-sort-up text-primary' : 'fa fa-sort-down text-primary';
  }  

}
