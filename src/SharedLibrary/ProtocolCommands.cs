namespace SharedLibrary
{
    /// Định nghĩa các hằng số protocol dùng chung cho cả Client và Server
    public class ProtocolCommands
    {
        // --- Chiều từ Client -> Server ---
        public const string FIRST_LOGIN_REGISTER = "FIRST_LOGIN_REGISTER";
        public const string LOGIN_ATTEMPT = "LOGIN_ATTEMPT";
        public const string UPLOAD = "UPLOAD";
        public const string DOWNLOAD = "DOWNLOAD";
        public const string DELETE_FILE = "DELETE_FILE";
        public const string PING = "PING";
        public const string QUIT = "QUIT";
        // --- Chiều từ Server -> Client ---
        public const string LOGIN_SUCCESS = "LOGIN_SUCCESS";
        public const string LOGIN_FAIL = "LOGIN_FAIL";
        public const string READY_FOR_UPLOAD = "READY_FOR_UPLOAD";
        public const string UPLOAD_SUCCESS = "UPLOAD_SUCCESS";
        public const string UPLOAD_FAIL = "UPLOAD_FAIL";
        public const string DOWNLOADING = "DOWNLOADING";
        public const string DOWNLOAD_FAIL = "DOWNLOAD_FAIL";
        public const string DELETE_SUCCESS = "DELETE_SUCCESS";
        public const string DELETE_FAIL = "DELETE_FAIL";
        public const string PONG = "PONG";
        public const string UNKNOWN_COMMAND = "UNKNOWN_COMMAND";
        public const string LOGOUT = "LOGOUT";
    }
}
