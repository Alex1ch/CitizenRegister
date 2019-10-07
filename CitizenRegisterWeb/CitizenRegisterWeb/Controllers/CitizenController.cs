using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using CitizenRegisterWeb.RequestMessages;
using CitizenRegisterWeb.Support;
using FastReport;
using FastReport.Export.Pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CitizenRegisterWeb.Controllers
{
    /// <summary>
    /// API Controller for read/write/update of Citizen entries
    /// </summary>
    [Route("api/[controller]")]
    public class CitizenController : ControllerBase
    {
        private DataBaseConfiguration _dbConfiguration;

        public CitizenController(IConfiguration configuration, DataBaseConfiguration dbConfiguration)
        {
            _dbConfiguration = dbConfiguration;
        }


        /// <summary>
        /// Get citizen entry by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Citizen>> Get([FromQuery] int id)
        {
            using (var dbConnection = _dbConfiguration.GetConnection() as DbConnection)
            {
                // Open connection and select citizen with specified ID
                // Query formed using just interpolation because there is no need for escaping integer
                dbConnection.Open();
                var result = await _dbConfiguration.NewCommand($"SELECT id, surname, name, middlename, birthdate FROM Citizen WHERE Id={ id }", dbConnection).ExecuteReaderAsync();
                if (!result.HasRows)
                {
                    dbConnection.Close();
                    return NotFound();
                }

                // Forming JSON result
                var citizen = new Citizen();
                result.Read();
                citizen.Id = result.GetInt32(0);
                citizen.Surname = result.GetString(1).Trim();
                citizen.Name = result.GetString(2).Trim();
                citizen.MiddleName = result.GetString(3).Trim();
                citizen.BirthDate = result.GetDateTime(4);

                dbConnection.Close();
                return Ok(citizen);
            }
        }

        
        /// <summary>
        /// Delete citizen with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            using (var dbConnection = _dbConfiguration.GetConnection() as DbConnection)
            {
                // Open connection and select citizen with specified ID
                // Query formed using just interpolation because there is no need for escaping integer
                dbConnection.Open();
                var result = await _dbConfiguration.NewCommand($"DELETE FROM Citizen WHERE Id={ id }", dbConnection).ExecuteNonQueryAsync();
                if (result == 0)
                {
                    dbConnection.Close();
                    return NotFound();
                }
                
                dbConnection.Close();
                return Ok();
            }
        }


        /// <summary>
        /// Request for citizens entries which satisfy search parameters
        /// </summary>
        /// <param name="citizen">search parameters (body)</param>
        /// <param name="count">number of entries (query)</param>
        /// <param name="offset">offset (query)</param>
        /// <returns></returns>
        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Citizen>>> Find([FromBody] CitizenSearchForm citizen, [FromQuery] int count = 100, [FromQuery] int offset = 0)
        {
            var result = await FindCitizens(citizen, count, offset);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        /// <summary>
        /// Create new entry of citizen
        /// </summary>
        /// <param name="citizen"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Post([FromBody] Citizen citizen)
        {
            // Check if form correct
            if (!citizen?.IsValid ?? true) return BadRequest();
                
            using (var dbConnection = _dbConfiguration.GetConnection() as DbConnection)
            {
                dbConnection.Open();
                
                var insertCommand = _dbConfiguration.NewCommand(
                    "INSERT INTO Citizen(surname, name, middlename, birthdate) " +
                    "VALUES (@surname, @name, @middlename, @birthdate);", 
                        dbConnection);

                // Setting Values for parameters
                insertCommand.Parameters.Add(_dbConfiguration.CreateStringParameter(insertCommand, citizen.Surname, "surname"));
                insertCommand.Parameters.Add(_dbConfiguration.CreateStringParameter(insertCommand, citizen.Name, "name"));
                insertCommand.Parameters.Add(_dbConfiguration.CreateStringParameter(insertCommand, citizen.MiddleName ?? "", "middlename"));
                insertCommand.Parameters.Add(_dbConfiguration.CreateDateParameter(insertCommand, citizen.BirthDate, "birthdate"));

                // Execute command
                var result = await insertCommand.ExecuteNonQueryAsync();

                // Check if rows were affected
                if (result == 0)
                {
                    dbConnection.Close();
                    return BadRequest();
                }
                dbConnection.Close();
                return Ok();
            }
        }


        /// <summary>
        /// Change values of citizen object
        /// </summary>
        /// <param name="citizen">Json object with id and values to change</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put([FromBody] Citizen citizen)
        {
            // Id is mandatory
            if (citizen.Id == 0)
                return BadRequest();

            using (var dbConnection = _dbConfiguration.GetConnection() as DbConnection)
            {
                // Making List for values which will be changed
                var values = new List<string>();
                
                if (!string.IsNullOrEmpty(citizen.Surname)) values.Add("surname=@surname");
                if (!string.IsNullOrEmpty(citizen.Name)) values.Add("name=@name");
                if (citizen.MiddleName != null) values.Add("middlename=@middlename");
                if (citizen.BirthDate != default(DateTime)) values.Add("birthdate=@birthdate");

                if (values.Count == 0)
                    return BadRequest();

                var valuesString = values[0];

                for (int i = 1; i < values.Count; i++)
                {
                    valuesString += $", { values[i] }";
                }
                
                var updateCommand =_dbConfiguration.NewCommand(
                        $"UPDATE citizen SET { valuesString } WHERE Id={ citizen.Id }", dbConnection);

                // Setting Values of Parameters in command 
                if (!string.IsNullOrEmpty(citizen.Surname))
                    updateCommand.Parameters.Add(
                        _dbConfiguration.CreateStringParameter(updateCommand, citizen.Surname, "surname"));
                if (!string.IsNullOrEmpty(citizen.Name))
                    updateCommand.Parameters.Add(
                        _dbConfiguration.CreateStringParameter(updateCommand, citizen.Name, "name"));
                if (citizen.MiddleName != null)
                    updateCommand.Parameters.Add(
                        _dbConfiguration.CreateStringParameter(updateCommand, citizen.MiddleName, "middlename"));
                if (citizen.BirthDate != default(DateTime))
                    updateCommand.Parameters.Add(
                        _dbConfiguration.CreateDateParameter(updateCommand, citizen.BirthDate, "birthdate"));

                dbConnection.Open();
                var result = await updateCommand.ExecuteNonQueryAsync();
                if (result == 0)
                {
                    dbConnection.Close();
                    return BadRequest();
                }

                dbConnection.Close();
                return Ok();
            }
        }


        /// <summary>
        /// Request for report of citizens entries which satisfy search parameters
        /// </summary>
        /// <param name="citizen">search parameters (body)</param>
        /// <param name="count">number of entries (query)</param>
        /// <param name="offset">offset (query)</param>
        /// <returns>PDF file</returns>
        [HttpGet]
        [Route("report")]
        [Consumes("application/json")]
        public async Task<IActionResult> Report([Bind("Surname", "Name", "MiddleName", "BirthDateAfter", "BirthDateBefore")] CitizenSearchForm citizen,
            [FromQuery] int count = 100, [FromQuery] int offset = 0)
        {
            Report report = new Report();
            report.Load("wwwroot/FastReport/Templates/Simple List.frx");

            var list = await FindCitizens(citizen, count, offset);
            if (list == null) return NotFound();

            DataSet ds = new DataSet();
            DataTable t = new DataTable("Citizen");

            ds.Tables.Add(t);

            foreach (var propInfo in typeof(Citizen).GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                t.Columns.Add(propInfo.Name, ColType);
            }

            //go through each property on T and add each value to the table
            foreach (Citizen item in list)
            {
                DataRow row = t.NewRow();

                foreach (var propInfo in typeof(Citizen).GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            report.RegisterData(ds, "Register");

            report.Prepare();
            PDFExport export = new PDFExport();

            var stream = new MemoryStream();
            export.Export(report, stream);

            stream.Position = 0;

            return File(stream,"application/pdf","CitizenReport.pdf");
        }


        public async Task<List<Citizen>> FindCitizens(CitizenSearchForm citizen, int count, int offset)
        {

            using (var dbConnection = _dbConfiguration.GetConnection() as DbConnection)
            {
                string whereString = string.Empty;
                // if request have body, it will be used for filter
                if (citizen != null)
                {
                    // Making List for values which used for filter
                    var values = new List<string>();

                    if (!string.IsNullOrEmpty(citizen.Surname)) values.Add("surname=@surname");
                    if (!string.IsNullOrEmpty(citizen.Name)) values.Add("name=@name");
                    if (!string.IsNullOrEmpty(citizen.MiddleName)) values.Add("middlename=@middlename");
                    if (citizen.BirthDate != default(DateTime))
                        values.Add("birthdate=@birthdate");
                    else
                    {
                        if (citizen.BirthDateAfter != default(DateTime)) values.Add("birthdate >= @birthdateafter");
                        if (citizen.BirthDateBefore != default(DateTime)) values.Add("birthdate <= @birthdatebefore");
                    }

                    // Forming WHERE part of request
                    if (values.Count != 0)
                    {
                        whereString = $"WHERE {values[0]}";

                        for (int i = 1; i < values.Count; i++)
                        {
                            whereString += $" AND {values[i]}";
                        }
                    }
                }

                // Some DB providers uses offset at the end of SQL query and some,
                // like informix, uses skip right after SELECT operator
                var offsetAfterSelect = _dbConfiguration.OffsetAfterSelect;

                // New Command
                var findCommand = _dbConfiguration.NewCommand(
                        $"SELECT {(offsetAfterSelect ? _dbConfiguration.OffsetKeyWord(offset) : string.Empty)} id, surname, name, middlename, birthdate FROM citizen { whereString } " +
                        $"{ (offsetAfterSelect ? string.Empty : _dbConfiguration.OffsetKeyWord(offset)) } " +
                        $"{ _dbConfiguration.LimitKeyWord(count) };", dbConnection);

                // Setting Values of Parameters in command 
                if (citizen != null)
                {
                    if (!string.IsNullOrEmpty(citizen.Surname))
                        findCommand.Parameters.Add(
                            _dbConfiguration.CreateStringParameter(findCommand, citizen.Surname, "surname"));
                    if (!string.IsNullOrEmpty(citizen.Name))
                        findCommand.Parameters.Add(
                            _dbConfiguration.CreateStringParameter(findCommand, citizen.Name, "name"));
                    if (!string.IsNullOrEmpty(citizen.MiddleName))
                        findCommand.Parameters.Add(
                            _dbConfiguration.CreateStringParameter(findCommand, citizen.MiddleName, "middlename"));
                    if (citizen.BirthDate != default(DateTime))
                        findCommand.Parameters.Add(
                            _dbConfiguration.CreateDateParameter(findCommand, citizen.BirthDate, "birthdate"));
                    else
                    {
                        if (citizen.BirthDateAfter != default(DateTime))
                            findCommand.Parameters.Add(
                                _dbConfiguration.CreateDateParameter(findCommand, citizen.BirthDateAfter, "birthdateafter"));
                        if (citizen.BirthDateBefore != default(DateTime))
                            findCommand.Parameters.Add(
                                _dbConfiguration.CreateDateParameter(findCommand, citizen.BirthDateBefore, "birthdatebefore"));
                    }
                }

                // Execution of command
                dbConnection.Open();

                var result = await findCommand.ExecuteReaderAsync();

                if (!result.HasRows)
                {
                    dbConnection.Close();
                    return null;
                }

                // forming list result
                var resultList = new List<Citizen>();

                while (result.Read())
                {
                    var newCitizen = new Citizen();
                    newCitizen.Id = result.GetInt32(0);
                    newCitizen.Surname = result.GetString(1).Trim();
                    newCitizen.Name = result.GetString(2).Trim();
                    newCitizen.MiddleName = result.GetString(3).Trim();
                    newCitizen.BirthDate = result.GetDateTime(4);

                    resultList.Add(newCitizen);
                }

                dbConnection.Close();
                return resultList;
            }
        }
    }
}
