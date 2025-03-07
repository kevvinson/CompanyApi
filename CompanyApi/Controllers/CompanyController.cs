﻿using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();

        [HttpPost]
        public ActionResult<Company> Create(CreateCompanyRequest request)
        {
            if (companies.Exists(company => company.Name.Equals(request.Name)))
            {
                return BadRequest();
            }
            Company companyCreated = new Company(request.Name);
            companies.Add(companyCreated);
            return StatusCode(StatusCodes.Status201Created, companyCreated);
        }

        [HttpDelete]
        public void ClearData()
        {
            companies.Clear();
        }

        [HttpGet("{id}")]
        public ActionResult<Company> GetCompany(string id)
        {
            Company company = companies.Find(x => x.Id == id);
            if(company==null)
            {
                return NotFound();
            }
            else
            {
                return Ok(company);
            }
        }

        [HttpGet]
        public ActionResult<List<Company>> GetCompanyInSpecificPage([FromQuery]int? pagesize, [FromQuery]int? pageindex)
        {
            if ((pagesize == null) || (pageindex == null)) return companies;
            else
            {
                List<Company> companiesInSpecificPage = new List<Company>();
                int start = (int)(pagesize * (pageindex - 1));
                int end = (int)(start + pagesize);
                for (int i = start; i < end; i++)
                {
                    companiesInSpecificPage.Add(companies[i]);
                }
                return Ok(companiesInSpecificPage);
            }
        }

        [HttpPost("{id}")]
        public ActionResult<Employee> AddEmployee(CreateEmployeeRequest request, string id)
        {
            Company company = companies.Find(x => x.Id == id);
            if(company==null)
            {
                return NotFound();
            }
            if ((company.Employees.Find(x => x.Name == request.Name)) != null)
            {
                return BadRequest();
            }
            Employee newEmployee = new Employee(request.Name);
            company.Employees.Add(newEmployee);
            return Ok(newEmployee);
        }

        [HttpDelete("{company_id}/employees/{employee_id}")]
        public ActionResult DeleteEmployee(string company_id, string employee_id)
        {
            Company company = companies.Find(x => x.Id == company_id);
            if (company == null)
            {
                return NotFound();
            }
            if ((company.Employees.Find(x => x.Id == employee_id)) == null)
            {
                return NotFound();
            }

            company.Employees.RemoveAll(x => x.Id == employee_id);
            return NoContent();
        }
    }
}
