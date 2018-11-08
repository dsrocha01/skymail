using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymail_PortalCorretor.Dao
{
    class Contact
    {
        public List<Object> GetListContact()
        {
            List<Object> list = new List<Object>();
            string qry = "select * from contact where " +
            "contact.customertypecode = @CustomerTypeCode " +
            "and not contact.nickname like '%PAGADORIA%' " +
            "and contact.pjo_datadeinicio >=  @Data";

            AdoHelper db = new AdoHelper(ConfigurationManager.ConnectionStrings["WebPortal"].ConnectionString);
            SqlDataReader reader = db.ExecDataReader(qry,
                "@CustomerTypeCode", 200001,
                "@Data", new DateTime(2018, 10, 31));
            while (reader.Read())
            {
                list.Add(reader.GetValue(reader.GetOrdinal("emailaddress1")));
                // Get row of data from rdr
            }
            return list;
        }
    }
}
