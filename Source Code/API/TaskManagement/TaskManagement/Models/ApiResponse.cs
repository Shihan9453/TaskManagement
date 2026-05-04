namespace TaskManagement.Models
{
    public class ApiResponse
    {

        public int Status { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public object Data { get; set; }

        public ApiResponse(int intStatus, string strMsg, string strDetails, object objData = null)
        {
            Status = intStatus;
            Message = strMsg;
            Details = strDetails;
            Data = objData;
        }

    }
}
