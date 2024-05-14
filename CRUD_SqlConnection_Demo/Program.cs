
using System;
using System.Data.SqlClient;

namespace CRUD_SqlConnection_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string response = "";
            while (response != "exit")
            {
                Console.Write(">Select from the given options<\n");
                Console.WriteLine(">>emp--\t\tDISPLAYS THE EMPLOYEE DATABASE MENU");
                Console.WriteLine(">>exit--\tEXIT THE PROGRAM");
                Console.WriteLine(">>clear--\tCLEAR THE CONSOLE WINDOW");
                response = Console.ReadLine();
                switch (response.ToLower())
                {
                    case "emp":
                        employeeMenu();
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Exiting the Program!\n");
                        break;

                }
            }
        }
        static void employeeMenu()
        {
            Console.WriteLine("\n-------------Menu-------------");
            Console.WriteLine("Type 'add' to Create all employees.");
            Console.WriteLine("Type 'display' to Read an employee.");
            Console.WriteLine("Type 'update' to Update an employee.");
            Console.WriteLine("Type 'del' to Delete an employee.");
            Console.WriteLine("------------------------------\n");
            string response = "";
            while (response != "exit")
            {
                Console.Write("\n\t\t\t<EmployeeMenu>");
                Console.Write("\n<Enter any CRUD operation you want to perform (add || display || update || del || exit)>");
                response = Console.ReadLine();
                switch (response.ToLower())
                {
                    case "add":
                        Console.WriteLine("\n\tRECORD INSERTION (CREATE)\n");
                        insertEmployee();
                        break;
                    case "display":
                        Console.WriteLine("\n\tDISPLAY RECORD (READ)\n");
                        readEmployee();
                        break;
                    case "update":
                        Console.WriteLine("\n\tRECORD UPDATION (UPDATE)\n");
                        updateEmployee();
                        break;
                    case "del":
                        Console.WriteLine("\n\tRECORD DELETION (DELETE)\n");
                        delEmployee();
                        break;
                    case "exit":
                        Console.WriteLine("\nReturning to Main Menu...\n");
                        break;
                    default:
                        Console.WriteLine("Invalid Command\n");

                        break;
                }
            }
        }
        static void insertEmployee()
        {
            var connection = new SqlConnection(@"Data Source=Wajeeha;Initial Catalog=Employee;Integrated Security=True");
            connection.Open();
            Console.WriteLine("Connection Opened Successfully!\n");
            try
            {
                //TAKE RECORD FROM USER
                Console.WriteLine("Enter Employee Id");
                int id = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter Employee Name:");
                string name = Console.ReadLine();
                Console.WriteLine("Enter Employee Phone:");
                int phone = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter Employee Address:");
                string addr = Console.ReadLine();
                Console.WriteLine("Enter Employee Salary:");
                int sal = int.Parse(Console.ReadLine());

                //INSERT DATA INTO TABLE
                string insertQuery = "INSERT INTO Emp_Data(Emp_Id, Emp_Name, Emp_Phone, Emp_Address, Emp_Salary) " +
                                     "VALUES(@Id, @Name, @Phone, @Address, @Salary)";
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Address", addr);
                command.Parameters.AddWithValue("@Salary", sal);
                command.ExecuteNonQuery();
                Console.WriteLine("Data Entered in the Table Successfully!\n");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("\nConnection Closed Successfully!\n");
                }
            }
            Console.ReadKey();
        }
        static void readEmployee()
        {
            var connection = new SqlConnection(@"Data Source=Wajeeha;Initial Catalog=Employee;Integrated Security=True");
            connection.Open();
            Console.WriteLine("Connection Opened Successfully!\n");

            try
            {
                string selQuery = "SELECT Emp_Id, Emp_Name, Emp_Salary, Emp_Address, Emp_Phone FROM Emp_Data";
                SqlCommand com = new SqlCommand(selQuery, connection);

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Employee Data:");
                       /* Console.WriteLine("\tRecord Number: ");
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader.GetName(i) + "\t"); // Displaying column names
                        }
                        Console.WriteLine(); */

                       // int j = 1;
                        while (reader.Read())
                        {
                            //Console.Write("\t{0}\t", j); // Displaying record number
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader.GetValue(i) + "\t"); // Displaying record values
                            }
                           // j++;
                            Console.WriteLine(); // Move to the next line after displaying record
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found in the database.\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("\nConnection Closed Successfully!\n");
                }
            }
            Console.ReadKey();
        }

        
        

        static void updateEmployee()
        {
            var connection = new SqlConnection(@"Data Source=Wajeeha;Initial Catalog=Employee;Integrated Security=True");
            connection.Open();
            Console.WriteLine("Connection Opened Successfully!\n");
            //UPDATE DATA IN DB
            try
            {
                Console.WriteLine("Enter Employee Name you want to Update:");
                string n = Console.ReadLine();
                Console.WriteLine("Enter Employee Id whose data you want to update:");
                int eid;
                if (!int.TryParse(Console.ReadLine(), out eid))
                {
                    Console.WriteLine("Invalid Employee Id. Please enter a valid integer value.");
                    return;
                }

                // Check if the provided ID exists in the table
                string selectQuery = "SELECT COUNT(*) FROM Emp_Data WHERE Emp_Id = @Id";
                SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                selectCommand.Parameters.AddWithValue("@Id", eid);
                int count = (int)selectCommand.ExecuteScalar();

                if (count == 0)
                {
                    Console.WriteLine("Error: Employee with provided ID does not exist.");
                    return;
                }

                // If the provided ID exists, proceed with the update
                string updateQuery = "UPDATE Emp_Data SET Emp_Name = @NewName WHERE Emp_Id = @Id";
                SqlCommand command1 = new SqlCommand(updateQuery, connection);
                command1.Parameters.AddWithValue("@NewName", n);
                command1.Parameters.AddWithValue("@Id", eid);
                command1.ExecuteNonQuery();
                Console.WriteLine("Data Updated Successfully!\n");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Connection Closed Successfully!\n");
                }
            }
            Console.WriteLine("");
            Console.ReadKey();

        }

        static void delEmployee()
        {
            var connection = new SqlConnection(@"Data Source=Wajeeha;Initial Catalog=Employee;Integrated Security=True");
            connection.Open();
            Console.WriteLine("Connection Opened Successfully!\n");
            //DELETE DATA FROM DB
            try
            {
                Console.WriteLine("Enter the Employee ID to delete:");
                int empId = Convert.ToInt32(Console.ReadLine());
                string deleteQuery = "DELETE FROM Emp_Data WHERE Emp_Id=@emp_id";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@emp_id", empId);
                deleteCommand.ExecuteNonQuery();
                Console.WriteLine("Data Deleted Successfully!\n");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Connection Closed Successfully!\n");
                }
            }

            Console.ReadKey();
        }

    }
}








