using Curso.Api.Entities;

namespace Curso.Api{
    public class Data{
        public List<Customer> Customers{get; set;}
        static private Data _data;

        private Data(){
            this.Customers = new List<Customer>{
                new Customer{
                    Id = 1,
                    Name = "Pedro",
                    Cpf = "123456789123",
                    Addresses = new List<Address>{
                        new Address{
                            Id = 1,
                            Street = "aquela pepe",
                            City = "ali ó pai"

                        }
                    }
                },
                new Customer{
                    Id = 2,
                    Name = "Pepino",
                    Cpf = "123999789993",
                    Addresses = new List<Address>{
                        new Address{
                            Id = 2,
                            Street = "aquela",
                            City = "ali ó"

                        },
                        new Address{
                            Id = 3,
                            Street = "aqueluio",
                            City = "Céu"

                        }
                    }
                }
            };
        }

        public static Data getInstance(){
            //ou colocar return _data ??= new Data();
            if (_data == null){
                _data = new Data();
            }
            return _data;
        }

    }
}