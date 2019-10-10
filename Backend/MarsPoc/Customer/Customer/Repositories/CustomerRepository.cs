using Common.Base;
using Common.Interfaces;
using Common.Models;
using Customer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Customer.Repositories
{
    public class CustomerRepository : RepositoryBase<CustomerModel, CustomerRepository>
    {
        private readonly ILogHandler logHandler;
        private readonly PatientRepository patientRepository;

        public CustomerRepository(
            DbContextOptions<CustomerRepository> options,
            PatientRepository patientRepository,
            ILogHandler logHandler)
            : base(options, logHandler)
        {
            this.patientRepository = patientRepository;
            this.logHandler = logHandler;
        }

        public override async Task<CustomerModel> GetModelAsync(int id)
        {
            var cust = await base.GetModelAsync(id);
            AddPatients(cust);
            return cust;
        }

        private void AddPatients(CustomerModel cust)
        {
            if (cust != null)
            {
                var petList = this.patientRepository.SearchPatients(cust.Id);
                cust.Patients = petList?.ToArray();
            }
        }

        public override async Task<IEnumerable<object>> SearchAllModelAsync(IEnumerable<int> idList)
        {
            if (idList == null || !idList.Any())
                return null;

            return await Task.Run(() =>
            {
                var itemList = (from item in this.ModelSet
                               where idList.Contains(item.Id)
                               select new { item.Id, item.Name, item.ImageUrl }).ToArray();
                return itemList;
            });
        }

        public async Task<MarsResponse> GetAllCustomersAsync()
        {
            return await Task.Run(() =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var itemList = (from item in this.ModelSet
                               select new { item.Id, item.Name, item.ImageUrl }).ToArray();

                stopwatch.Stop();
                this.logHandler.LogMetric(this.GetType().ToString() + " GetAllCustomersAsync", stopwatch.ElapsedMilliseconds);

                HttpStatusCode code = HttpStatusCode.OK;
                if (itemList == null || itemList.Count() == 0)
                    code = HttpStatusCode.NotFound;
                return MarsResponse.GetResponse(itemList, code);
            });
        }

        protected override IEnumerable<CustomerModel> FilterSearch(CustomerModel customer)
        {
            List<Func<CustomerModel, bool>> filterList = new List<Func<CustomerModel, bool>>();

            //if (!string.IsNullOrWhiteSpace(customer.Name))
            //    filterList.Add(c => !string.IsNullOrWhiteSpace(customer.Name) && c.Name.Contains(customer.Name, StringComparison.InvariantCultureIgnoreCase));

            //if (!string.IsNullOrWhiteSpace(customer.ContactNumber))
            //    filterList.Add(c => c.ContactNumber.Contains(customer.ContactNumber, StringComparison.InvariantCultureIgnoreCase));

            //if (!string.IsNullOrWhiteSpace(customer.Address))
            //    filterList.Add(c => c.Address.Contains(customer.Address, StringComparison.InvariantCultureIgnoreCase));

            var customerList = this.ModelSet.Where(c => !string.IsNullOrWhiteSpace(customer.Name) && c.Name.Contains(customer.Name, StringComparison.InvariantCultureIgnoreCase)).ToArray();

            foreach (var cust in customerList ?? Enumerable.Empty<CustomerModel>())
                AddPatients(cust);

            return customerList;
        }
    }
}
