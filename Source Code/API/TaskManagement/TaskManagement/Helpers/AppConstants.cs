namespace TaskManagement.Helpers
{
    public class AppConstants
    {

        // Status Codes 
        public readonly int STATUS_CODE_OK = 200;
        public readonly int STATUS_CODE_CREATED = 201;
        public readonly int STATUS_CODE_BAD_REQUEST = 400;
        public readonly int STATUS_CODE_UNAUTHORIZED = 401;
        public readonly int STATUS_CODE_NOT_FOUND = 404;

        // Status 
        public readonly string STATUS_OK = "OK";
        public readonly string STATUS_CREATED = "CREATED";
        public readonly string STATUS_BAD_REQUEST = "BAD REQUEST";
        public readonly string STATUS_UNAUTHORIZED = "UNAUTHORIZED";
        public readonly string STATUS_NOT_FOUND = "NOT FOUND";

        // Success Messages 
        public readonly string SUCCESS_INSERTED = "Record successfully saved.";
        public readonly string SUCCESS_UPDATED = "Record successfully updated.";
        public readonly string SUCCESS_DELETED = "Record successfully deleted.";
        public readonly string SUCCESS_RECORDS_FOUND = "Records found.";
        public readonly string SUCCESS_LOGIN_SUCCESS = "Login successful.";
        public readonly string SUCCESS_LOGOUT_SUCCESS = "Logout successful.";

        // Error Messages
        public readonly string ERROR_NO_RCORDS_FOUND = "No records found.";
        public readonly string ERROR_NO_RCORDS_FOUND_TO_UPDATE = "No record found to update.";
        public readonly string ERROR_NO_RCORDS_FOUND_TO_DELETE = "No record found to delete.";
        public readonly string ERROR_USERNAME_PASSWORD_REQUIRED = "Username and password are required.";
        public readonly string ERROR_USERNAME_INVALID = "Invalid username.";
        public readonly string ERROR_PASSWORD_INVALID = "Invalid password.";
        public readonly string ERROR_USER_NOT_FOUND = "User not found.";

        // Task History Actions 
        public readonly string TASK_HISTORY_ACTION_INSERTED = "Task created successfully.";
        public readonly string TASK_HISTORY_ACTION_UPDATED = "Task updated successfully.";

        // Task Status 
        public readonly string TASK_STATUS_COMPLETED = "Completed"; 
        public List<string> lstStatuses = new List<string>() { "To Do", "In Progress", "On Hold", "Completed" };

        // Task Priority Level
        public List<string> lstPriorityLevels = new List<string>() { "Low", "Medium", "High" };

    }
}
