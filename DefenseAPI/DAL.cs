using DefenseAPI.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DefenseAPI
{
    public class DAL
    {
        SqlConnection _connection = null;
        SqlCommand _cmd = null;

        public static IConfiguration configuration { get; set; }
        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            configuration = builder.Build();
            return configuration.GetConnectionString("SQLServer");

        }

        public bool Insert(Personnel model)
        {
            int id = 0;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _cmd = _connection.CreateCommand();
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.CommandText = "[dbo].[usp_InsertPersonnel]";
                //_cmd.Parameters.AddWithValue("@PersId", model.PersId);
                _cmd.Parameters.AddWithValue("@CoyId", model.RankId);
                _cmd.Parameters.AddWithValue("@RankId", model.CoyId);
                _cmd.Parameters.AddWithValue("@TypeOfPersonnel", model.TypeOfPersonnel);
                _cmd.Parameters.AddWithValue("@PersNo", model.PersNo);
                _cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                _cmd.Parameters.AddWithValue("@LastName", model.LastName);
                _cmd.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
                _cmd.Parameters.AddWithValue("@PermanentAddress", model.PermanentAddress);
                _connection.Open();
                id = _cmd.ExecuteNonQuery();
                _connection.Close();
            }
            return id > 0 ? true : false;
        }
        public Personnel GetPersonnel(int PersnId)
        {
            Personnel user = null;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("usp_GetUserDetails", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PersnId", PersnId);
                _connection.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    if (sdr.Read())
                    {
                        user = new Personnel()
                        {
                            PersId = Convert.ToInt32(sdr["PersId"]),
                            TypeOfPersonnel = Convert.ToString(sdr["TypeOfPersonnel"]),
                            PersNo = Convert.ToString(sdr["PersNo"]),
                            FirstName = Convert.ToString(sdr["FirstName"]),
                            LastName = Convert.ToString(sdr["LastName"]),
                            DateOfBirth = Convert.ToString(sdr["DateOfBirth"]),
                            PermanentAddress = Convert.ToString(sdr["PermanentAddress"]),

                        };
                    }
                }
            }
            return user;
        }

        public IEnumerable<Personnel> GetAllPersonnel()
        {
            Personnel person = null;
            List<Personnel> lst = new List<Personnel>();

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("usp_GetAllUser", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _connection.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        person = new Personnel();
                        person.PersId = Convert.ToInt32(sdr["PersId"]);
                        person.TypeOfPersonnel = Convert.ToString(sdr["TypeOfPersonnel"]);
                        person.PersNo = Convert.ToString(sdr["PersNo"]);
                        person.FirstName = Convert.ToString(sdr["FirstName"]);
                        person.LastName = Convert.ToString(sdr["LastName"]);
                        person.DateOfBirth = Convert.ToString(sdr["DateOfBirth"]);
                        person.PermanentAddress = Convert.ToString(sdr["PermanentAddress"]);
                        lst.Add(person);
                    }
                    _connection.Close();
                }
            }
            return lst;
        }

        public int DeletePersonnel(int PersnId)
        {
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("usp_DeletePersonnel", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PersnId", PersnId);
                _connection.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        public int UpdatePesonnel(Personnel personnel)
        {
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("usp_UpdatePersonnel", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //cmd.Parameters.AddWithValue("@PersnId", personnel.PersId);
                cmd.Parameters.AddWithValue("@TypeOfPrson", personnel.TypeOfPersonnel);
                cmd.Parameters.AddWithValue("@PersNo", personnel.PersNo);
                cmd.Parameters.AddWithValue("@FirstName", personnel.FirstName);
                cmd.Parameters.AddWithValue("@LastName", personnel.LastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", personnel.DateOfBirth);
                cmd.Parameters.AddWithValue("@PermanentAddress", personnel.PermanentAddress);
                _connection.Open();
                return cmd.ExecuteNonQuery();

            }
        }

        public List<SelectListItem> GetRanks()
        {
            SelectListItem item = null;
            List<SelectListItem> lst = new List<SelectListItem>();
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("spGetAllRanks", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _connection.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        item = new SelectListItem();
                        item.Value = Convert.ToString(sdr["RankId"]);
                        item.Text = Convert.ToString(sdr["RankName"]);
                        lst.Add(item);
                    }
                }
            }
            return lst;
        }
        public List<SelectListItem> getCompany()
        {
            SelectListItem item = null;
            List<SelectListItem> lst = new List<SelectListItem>();
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[spGetAllCompanies]", _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _connection.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        item = new SelectListItem();
                        item.Value = Convert.ToString(sdr["CoyId"]);
                        item.Text = Convert.ToString(sdr["CoyName"]);
                        lst.Add(item);
                    }
                }
            }
            return lst;
        }

        public async Task<(bool IsAuthenticated, string Company)> LoginAsync(string persNo)
        {
            bool isAuthenticated = false;
            string company = null;

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand("usp_Login", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@PersNo", SqlDbType.NVarChar, 50) { Value = persNo });

                    var isAuthenticatedParam = new SqlParameter("@IsAuthenticated", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(isAuthenticatedParam);

                    var companyParam = new SqlParameter("@Company", SqlDbType.NVarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(companyParam);

                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    isAuthenticated = (bool)isAuthenticatedParam.Value;
                    company = companyParam.Value as string;
                }
            }

            return (isAuthenticated, company);
        }
    }
}


