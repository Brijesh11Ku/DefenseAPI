using DefenseAPI.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DefenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefenseController : ControllerBase 
    {

        private readonly DAL _DataAccess;

        public DefenseController(DAL DataAccess)
        {
            _DataAccess = DataAccess;
        }
        // GET: api/<DefenseController>
        [HttpGet("GetAllPersonnel")]
        public IActionResult GetAllPersonnel()
        {
            try
             {
                var result = _DataAccess.GetAllPersonnel();
                if(result!=null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();    
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET api/<DefenseController>/5
        [HttpGet("Get/{PersnId}")]
        public IActionResult Get(int PersnId)
        {
            try
            {
                var result = _DataAccess.GetPersonnel(PersnId);
                if(result!= null)
                {
                    return Ok(result);
                }
                else
                {
                        return BadRequest();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // POST api/<DefenseController>
        [HttpPost]
        public IActionResult Post(Personnel personnel)
        {
            try
            {
                bool result = _DataAccess.Insert(personnel);
                if (result)
                    return Ok(result);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // PUT api/<DefenseController>/5
        [HttpPut("UpdatePersonnel")]
        public IActionResult UpdatePersonnel(Personnel person)
        {
            try
            {
                if(!string.IsNullOrEmpty(person.FirstName) || !string.IsNullOrEmpty(person.LastName) || !string.IsNullOrEmpty(person.PermanentAddress)
                    || person.FirstName!="string" || person.LastName!="string" || person.PermanentAddress!="string")
                {
                    var result = _DataAccess.UpdatePesonnel(person);
                    if (result > 0)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    throw new Exception("Invalid Data!");
                }
                

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // DELETE api/<DefenseController>/5
        [HttpDelete("DeletePersonnel/{id}")]
        public IActionResult DeletePersonnel(int id)
        {
            try
            {
                var result = _DataAccess.DeletePersonnel(id);
                if(result>0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet("GetAllRanks")]
        public IActionResult GetAllRanks()
        {
            try
            {
                var result = _DataAccess.GetRanks();
                if (result != null)
                    return Ok(result);
                else 
                    return BadRequest();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet("GetCompany")]
        public IActionResult GetCompany()
        {
            try
            {
                var result = _DataAccess.getCompany();
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
