//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMODA_FRONT_HTTPS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class discount_code
    {
        public int id { get; set; }
        public System.DateTime c_time { get; set; }
        public int c_id { get; set; }
        public System.DateTime u_time { get; set; }
        public int u_id { get; set; }
        public string name { get; set; }
        public Nullable<int> amount { get; set; }
        public Nullable<int> deduction { get; set; }
        public Nullable<System.DateTime> start_time { get; set; }
        public Nullable<System.DateTime> end_time { get; set; }
        public int level { get; set; }
        public string remark { get; set; }
    }
}
