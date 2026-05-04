CREATE TABLE Users 
(
  id INT IDENTITY(1,1) NOT NULL,
  username VARCHAR(50) NOT NULL,
  user_password VARCHAR(15) NOT NULL,
  full_name VARCHAR(100) NOT NULL,
  PRIMARY KEY(id)
);

CREATE TABLE Tasks 
(
  id INT IDENTITY(1,1) NOT NULL,
  title VARCHAR(255) NOT NULL,
  description VARCHAR(255) NOT NULL,
  status VARCHAR(11) NOT NULL CHECK(status IN('To Do','In Progress', 'On Hold', 'Completed')),
  priority_level VARCHAR(6) NOT NULL CHECK(priority_level IN('Low','Medium','High')),
  assigned_person_id int NOT NULL,
  due_date DATETIME NOT NULL,
  completed_date DATETIME,
  remarks VARCHAR(255),
  PRIMARY KEY(id),
  FOREIGN KEY (assigned_person_id) REFERENCES Users(id)
);

CREATE TABLE TasksHistory
(
   id INT IDENTITY(1,1) NOT NULL,
   task_id INT NOT NULL,
   user_id INT NOT NULL,
   action VARCHAR(100) NOT NULL,
   date DATETIME DEFAULT GETDATE(),
   PRIMARY KEY(id),
   FOREIGN KEY(task_id) REFERENCES Tasks(id),
   FOREIGN KEY(user_id) REFERENCES Users(id)
);