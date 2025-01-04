using DataAccessDAL;
using System.Data;

namespace DataPassingBLL
{
    public class DataBLL
    {
        DataDAL DAL = new DataDAL();
        DataSet ds = new DataSet();
        public DataSet GetDataSet()
        {
            ds = DAL.GetDataSet();
            return ds;
        }
        public void RegisterUser(DataSet ds)
        {
            DAL.UpdateUser(ds);
        }
        public void Changes(DataSet ds)
        {
            DAL.UpdateDB(ds);
        }
    }
}
