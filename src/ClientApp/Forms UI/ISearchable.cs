using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Forms_UI
{
    /// <summary>
    /// Interface để đánh dấu UserControl hỗ trợ tìm kiếm
    /// </summary>
    public interface ISearchable
    {
        /// <summary>
        /// Tìm kiếm file theo từ khóa
        /// </summary>
        void SearchFiles(string keyword);
    }
}
