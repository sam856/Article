using MaqaletyCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaqaletyData
{
    public  interface IDataHelper<Table>
    {
        //Read
        List<Table> GetAllData();
        List<Table> GetDataByUser(int id);
        List<Table> Search(string SearchItem);
        Table find (int id);

        //Write
        int Add (Table table);
        int Update (Table table,int Id);
        int delete(int  id);

    }
}
