using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace WoodTransactions
{
    public class DataContext
    {
        string connStr = String.Empty;

        public List<DealValidateData> Result;

        public DataContext()
        {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);

            connStr = $@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {strWorkPath}\Database1.mdf; Integrated Security = True";
        }

        public void HandleData(List<DealValidateData> dataForHandle)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                try
                {
                    foreach (var data in dataForHandle)
                    {
                        using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM WoodDeal WHERE dealNumber = N'" + data.DealNumber + "'", connection))
                        {
                            sqlCommand.CommandType = CommandType.Text;

                            
                            var result = sqlCommand.ExecuteScalar();

                            string val = result == null ? "" : result.ToString();

                            if (String.IsNullOrEmpty(val))
                            {
                                AddData(data, connection);
                            }
                            else
                            {
                                var reader = sqlCommand.ExecuteReader();

                                if (reader.Read() && (!data.SellerName.Equals(reader["SellerName"].ToString()) ||
                                        !data.SellerInn.Equals(reader["SellerInn"].ToString()) ||
                                        !data.BuyerName.Equals(reader["BuyerName"].ToString()) ||
                                        !data.BuyerInn.Equals(reader["BuyerInn"].ToString()) ||
                                        !data.WoodVolumeBuyer.Equals(reader.GetDouble(5)) ||
                                        !data.WoodVolumeSeller.Equals(reader.GetDouble(6)) ||
                                        !data.DealDate.Equals(reader.GetDateTime(7))))
                                {
                                    reader.Close();

                                    DeleteData(data, connection);
                                    AddData(data, connection);
                                }

                                reader.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error database! " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void AddData(DealValidateData data, SqlConnection connection)
        {
            using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO WoodDeal VALUES (N'" +
            data.DealNumber + "', N'" +
            data.SellerName + "', N'" +
            data.SellerInn + "', N'" +
            data.BuyerName + "', N'" +
            data.BuyerInn + "', " +
            data.WoodVolumeBuyer + ", " +
            data.WoodVolumeSeller + ", '" +
            data.DealDate.ToString("yyyy-MM-dd") + "')", connection))
            {
                try
                {
                    sqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new Exception("Add Error", ex);
                }
            }
        }
        
        private void DeleteData(DealValidateData data, SqlConnection connection)
        {
            using (SqlCommand sqlCommand = new SqlCommand("DELETE WoodDeal WHERE dealNumber = N'" +
            data.DealNumber + "'", connection))
            {
                try
                {
                    sqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new Exception("Delete Error", ex);
                }
            }
        }
    }
}
