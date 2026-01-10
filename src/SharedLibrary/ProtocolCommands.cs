namespace SharedLibrary
{
    public class ProtocolCommands
    {
        // ============ AUTH (Client → Server) ============
        public const string FIRST_LOGIN_REGISTER = "FIRST_LOGIN_REGISTER";
        public const string LOGIN_ATTEMPT = "LOGIN_ATTEMPT";

        // ============ AUTH (Server → Client) ============
        public const string LOGIN_SUCCESS = "LOGIN_SUCCESS";
        public const string LOGIN_FAIL = "LOGIN_FAIL";

        // ============ FILE UPLOAD ============
        // Client → Server:
        public const string UPLOAD_REQ = "UPLOAD_REQ";

        // Server → Client:
        public const string READY_FOR_UPLOAD = "READY_FOR_UPLOAD"; 
        public const string UPLOAD_SUCCESS = "UPLOAD_SUCCESS";     
        public const string UPLOAD_FAIL = "UPLOAD_FAIL";           
        public const string QUOTA_EXCEEDED = "QUOTA_EXCEEDED";     // lỗi dung lượng
        public const string INVALID_PATH = "INVALID_PATH";
        public const string INVALID_FILENAME = "INVALID_FILENAME";

        // ============ FILE DOWNLOAD ============
        public const string DOWNLOAD = "DOWNLOAD"; 
        public const string DOWNLOADING = "DOWNLOADING"; 
        public const string DOWNLOAD_FAIL = "DOWNLOAD_FAIL";
        public const string FILE_NOT_FOUND = "FILE_NOT_FOUND";
        public const string ACCESS_DENIED = "ACCESS_DENIED";

        // ============ DELETE FILE ============
        public const string DELETE_FILE = "DELETE_FILE";
        public const string DELETE_SUCCESS = "DELETE_SUCCESS";
        public const string DELETE_FAIL = "DELETE_FAIL";

        // ============ FOLDER OPERATIONS ============
        public const string CREATE_FOLDER = "CREATE_FOLDER";
        public const string CREATE_FOLDER_SUCCESS = "CREATE_FOLDER_SUCCESS";
        public const string CREATE_FOLDER_FAIL = "CREATE_FOLDER_FAIL";

        public const string DELETE_FOLDER = "DELETE_FOLDER"; 
        public const string DELETE_FOLDER_SUCCESS = "DELETE_FOLDER_SUCCESS";
        public const string DELETE_FOLDER_FAIL = "DELETE_FOLDER_FAIL";

        // ============ FILE LISTING ============
        public const string LIST_FILES = "LIST_FILES";
        public const string LIST_FILES_SUCCESS = "LIST_FILES_SUCCESS";
        public const string LIST_FILES_FAIL = "LIST_FILES_FAIL";

        // ============ RENAME ============
        public const string RENAME_FILE = "RENAME_FILE";
        public const string RENAME_SUCCESS = "RENAME_SUCCESS";
        public const string RENAME_FAIL = "RENAME_FAIL";

        // ============ SYSTEM ============
        public const string PING = "PING";
        public const string PONG = "PONG";
        public const string QUIT = "QUIT";

        public const string UNKNOWN_COMMAND = "UNKNOWN_COMMAND";

        // tìm kiếm file
        public const string SEARCH_REQ = "SEARCH_REQ";      // Client gửi yêu cầu
        public const string SEARCH_SUCCESS = "SEARCH_SUCCESS"; // Server trả kết quả
        //Star file
        public const string STAR_FILE = "STAR_FILE";
        public const string STAR_SUCCESS = "STAR_SUCCESS";
        public const string STAR_FAIL = "STAR_FAIL";

        // ============ STORAGE INFO ============
        public const string GET_STORAGE_INFO = "GET_STORAGE_INFO";           // Client yêu cầu thông tin dung lượng
        public const string GET_STORAGE_INFO_SUCCESS = "GET_STORAGE_INFO_SUCCESS"; // Server trả kết quả
        public const string GET_STORAGE_INFO_FAIL = "GET_STORAGE_INFO_FAIL";   // Server lỗi


        public const string MOVE_TO_TRASH = "MOVE_TO_TRASH";           // Lệnh chuyển vào thùng rác
        public const string MOVE_TO_TRASH_SUCCESS = "MOVE_TO_TRASH_SUCCESS"; // Thành công

        public const string RESTORE_FILE = "RESTORE_FILE";             // Lệnh khôi phục
        public const string RESTORE_SUCCESS = "RESTORE_SUCCESS";

    }

}
