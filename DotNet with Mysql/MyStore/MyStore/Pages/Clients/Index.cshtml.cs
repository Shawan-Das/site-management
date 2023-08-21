using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace MyStore.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients= new List<ClientInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT* FROM clients";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetValue(0);
                                clientInfo.name ="" + reader.GetValue(1);
                                clientInfo.email =""+ reader.GetValue(2);
                                clientInfo.phone =""+ reader.GetValue(3);
                                clientInfo.address = "" + reader.GetValue(4);
                                clientInfo.createdAt="" + reader.GetValue(5).ToString();

                                listClients.Add(clientInfo);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        private SqlCommand SqlCommand(string sql, SqlConnection connection)
        {
            throw new NotImplementedException();
        }
    }

    public class ClientInfo
    {
        public String id;
        public String name;
        public String email;
        public String phone;
        public String address;
        public String createdAt;
    }
}
