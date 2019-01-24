using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Events;
using Domain.IEntity;
using Infrastructure.Repositories;
using NHibernate;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;
using Application = Microsoft.Office.Interop.Excel.Application;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;

namespace Infrastructure
{
    public class Parser
    {
        public void Start(List<EventDto> dtos)
        {
            //DataBaseHelper.CreateDB();
            DataBaseHelper.ClearDB();
            NHibernateHelper.Configure();
            SaveEvents(dtos);
            ToExcel();
        }


        private void ToExcel()
        {

            var excelApp = new Application();
            //Книга
            var workBook = excelApp.Workbooks.Add(System.Reflection.Missing.Value);
            //Таблица

            workBook.Worksheets.Add();
            workBook.Worksheets.Add();
            workBook.Worksheets.Add();
            workBook.Worksheets.Add();
            workBook.Worksheets.Add();
            workBook.Worksheets.Add();

            using (var repository = new NHRepository<Country>())
            {
                ToWorksheet(repository.GetAll().ToList(), workBook, excelApp);
            }

            using (var repository = new NHRepository<CityType>())
            {
                ToWorksheet(repository.GetAll().ToList(), workBook, excelApp);
            }

            using (var repository = new NHRepository<Region>())
            {
                ToWorksheet(repository.GetAll().ToList(), workBook, excelApp);
            }
            
            using (var repository = new NHRepository<City>())
            {
                ToWorksheet(repository.GetAll().ToList(), workBook, excelApp);
            }
            
            using (var repository = new NHRepository<Street>())
            {
                ToWorksheet(repository.GetAll().ToList(), workBook, excelApp);
            }
            
            using (var repository = new NHRepository<Address>())
            {
                ToWorksheet(repository.GetAll().ToList(), workBook, excelApp);
            }

            using (var repository = new NHRepository<Event>())
            {
                ToWorksheet(repository.GetAll().ToList(), workBook, excelApp);
            }


            excelApp.Worksheets[1].Select();
            //Вызываем созданную таблицу
            excelApp.Visible = true;
            excelApp.UserControl = true;
        }

        private void ToWorksheet(List<Country> table, Workbook workBook, Application excelApp)
        {
            string[] heads = new string[] { "Id", "Наименование" };

            var workSheet = (Worksheet)workBook.Worksheets.get_Item(1);
            workSheet.Name = "Country";
            
            for (int i = 0; i < heads.Length; i++)
            {
                //var xlRange1 = workSheet.get_Range("A1", "A");

                //xlRange1.Interior.Color = System.Drawing.Color.Yellow.ToArgb();

                excelApp.Cells[1, i + 1] = heads[i];
            }
            for (int i = 0; i < table.Count; i++)
            {
                excelApp.Cells[i + 2, 1] = table[i].Id.ToString();
                excelApp.Cells[i + 2, 2] = table[i].Name;
            }
            workSheet.Columns.AutoFit();
        }
        
        private void ToWorksheet(List<CityType> table, Workbook workBook, Application excelApp)
        {
            string[] heads = new string[] { "Id", "Наименование" };

            var workSheet = (Worksheet)workBook.Worksheets.get_Item(2);
            workSheet.Name = "CityType";
            excelApp.Worksheets[2].Select();
            for (int i = 0; i < heads.Length; i++)
                excelApp.Cells[1, i + 1] = heads[i];
            for (int i = 0; i < table.Count; i++)
            {
                excelApp.Cells[i + 2, 1] = table[i].Id.ToString();
                excelApp.Cells[i + 2, 2] = table[i].Name;
            }
            workSheet.Columns.AutoFit();
        }
        
        private void ToWorksheet(List<Region> table, Workbook workBook, Application excelApp)
        {
            string[] heads = new string[] { "Id", "Наименование", "Country" };

            var workSheet = (Worksheet)workBook.Worksheets.get_Item(3);
            workSheet.Name = "Region";
            excelApp.Worksheets[3].Select();
            for (int i = 0; i < heads.Length; i++)
                excelApp.Cells[1, i + 1] = heads[i];
            for (int i = 0; i < table.Count; i++)
            {
                excelApp.Cells[i + 2, 1] = table[i].Id.ToString();
                excelApp.Cells[i + 2, 2] = table[i].Name;
                excelApp.Cells[i + 2, 3] = table[i].Country.Id.ToString();
            }
            workSheet.Columns.AutoFit();
        }

        private void ToWorksheet(List<City> table, Workbook workBook, Application excelApp)
        {
            string[] heads = new string[] { "Id", "Наименование", "Country", "CityType" };

            var workSheet = (Worksheet)workBook.Worksheets.get_Item(4);
            workSheet.Name = "City";
            excelApp.Worksheets[4].Select();
            for (int i = 0; i < heads.Length; i++)
                excelApp.Cells[1, i + 1] = heads[i];
            for (int i = 0; i < table.Count; i++)
            {
                excelApp.Cells[i + 2, 1] = table[i].Id.ToString();
                excelApp.Cells[i + 2, 2] = table[i].Name;
                excelApp.Cells[i + 2, 3] = table[i].Region.Id.ToString();
                excelApp.Cells[i + 2, 4] = table[i].CityType.Id.ToString();
            }
            workSheet.Columns.AutoFit();
        }
        
        private void ToWorksheet(List<Street> table, Workbook workBook, Application excelApp)
        {
            string[] heads = new string[] { "Id", "Наименование", "City" };

            var workSheet = (Worksheet)workBook.Worksheets.get_Item(5);
            workSheet.Name = "Street";
            excelApp.Worksheets[5].Select();
            for (int i = 0; i < heads.Length; i++)
                excelApp.Cells[1, i + 1] = heads[i];
            for (int i = 0; i < table.Count; i++)
            {
                excelApp.Cells[i + 2, 1] = table[i].Id.ToString();
                excelApp.Cells[i + 2, 2] = table[i].Name;
                excelApp.Cells[i + 2, 3] = table[i].City.Id.ToString();
            }
            workSheet.Columns.AutoFit();
        }

        private void ToWorksheet(List<Address> table, Workbook workBook, Application excelApp)
        {
            string[] heads = new string[] { "Id", "House", "City" };

            var workSheet = (Worksheet)workBook.Worksheets.get_Item(6);
            workSheet.Name = "Address";
            excelApp.Worksheets[6].Select();
            for (int i = 0; i < heads.Length; i++)
                excelApp.Cells[1, i + 1] = heads[i];
            for (int i = 0; i < table.Count; i++)
            {
                excelApp.Cells[i + 2, 1] = table[i].Id.ToString();
                excelApp.Cells[i + 2, 2] = table[i].House;
                excelApp.Cells[i + 2, 3] = table[i].Street.Id.ToString();
            }
            workSheet.Columns.AutoFit();
        }

        private void ToWorksheet(List<Event> table, Workbook workBook, Application excelApp)
        {
            string[] heads = new string[] { "Id", "Наименование", "Информация", "Комментарий", "Дата проведения", "Адрес" };

            var workSheet = (Worksheet)workBook.Worksheets.get_Item(7);
            workSheet.Name = "Event";
            excelApp.Worksheets[7].Select();
            for (int i = 0; i < heads.Length; i++)
                excelApp.Cells[1, i + 1] = heads[i];
            for (int i = 0; i < table.Count; i++)
            {
                excelApp.Cells[i + 2, 1] = table[i].Id.ToString();
                excelApp.Cells[i + 2, 2] = table[i].Name;
                excelApp.Cells[i + 2, 3] = table[i].Info;
                excelApp.Cells[i + 2, 4] = table[i].Comment;
                excelApp.Cells[i + 2, 5] = table[i].Execution;
                excelApp.Cells[i + 2, 6] = table[i].Address.Id.ToString();

            }
            workSheet.Columns.AutoFit();
        }


        private void SaveEvents(List<EventDto> dtos)
        {
            using (var repository = new NHRepository<Country>())
            {
                foreach (var countryName in dtos.Select(x => x.Country).Distinct())
                {
                    repository.Add(new Country() {Name = countryName});
                }
            }

            using (var repository = new NHRepository<CityType>())
            {
                foreach (var cityTypeName in dtos.Select(x => x.CityType).Distinct())
                {
                    repository.Add(new CityType() {Name = cityTypeName});
                }
            }

            foreach (var dto in dtos)
            {
                Region region;
                using (var repository = new RegionRepository())
                {
                    region = repository.Get(dto.Country, dto.Region);
                    if (region == null)
                        region = repository.Add(new Region()
                        {
                            Name = dto.Region,
                            Country = GetFromRepo<Country>(dto.Country)
                        });
                }
                
                City city;
                using (var repository = new CityRepository())
                {
                    city = repository.Get(region.Id, dto.CityType, dto.City);
                    if (city == null)
                        city = repository.Add(new City()
                        {
                            Name = dto.City,
                            CityType = GetFromRepo<CityType>(dto.CityType),
                            Region = GetFromRepo<Region>(region.Id)
                        });
                }

                Street street;
                using (var repository = new StreetRepository())
                {
                    street = repository.Get(city.Id, dto.Street);
                    if (street == null)
                        street = repository.Add(new Street()
                        {
                            Name = dto.Street,
                            City = GetFromRepo<City>(city.Id),
                        });
                }

                Address address;
                using (var repository = new AddressRepository())
                {
                    address = repository.Get(street.Id, dto.House);
                    if (address == null)
                        address = repository.Add(new Address()
                        {
                            House = dto.House,
                            Street = GetFromRepo<Street>(street.Id)
                        });
                }

                using (var repository = new NHRepository<Event>())
                {
                    repository.Add(new Event()
                    {
                        Name = dto.EventName,
                        Info = dto.EventInfo,
                        Address = GetFromRepo<Address>(address.Id),
                        Comment = dto.EventComment,
                        Execution = dto.Execution
                    });
                }
            }

        }
        
        private T GetFromRepo<T>(string name) where T : NamedEntity
        {
            using (var repository = new NamedRepository<T>())
            {
                return repository.Get(name);
            }
        }
        private T GetFromRepo<T>(long id) where T : IEntity
        {
            using (var repository = new NHRepository<T>())
            {
                return repository.Get(id);
            }
        }

    }
}
