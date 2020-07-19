using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFD
{

    public class ShopContext : DbContext
    {

        public DbSet<Product> products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });//entity komutum sqlde nasıl göremk için ekledim
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory).UseMySql(@"server=localhost;port=3306;database=ShopDb;user=root;password=1532blmz");

            // //entity komutum sqlde nasıl göremk için ekledim bu satırı
        }
    }
    public class User
    {//one to many bir kullanıcının birden fazla adresi olabilir
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<Address> Addresses { get; set; }//elde ettiğim herhangi userin üzerinden adresses dediğim zaman adres bilgisi gelecek.bir userin birde nfazla adresi olabileceği için list
                                                    //herhang bir kullanıcı üzerinden addreses dersem o kullanıcının adersi gelir
        //bir kullanıcının birden fazla adresi olabilir. Birkaç tane adres yanlızca bir usere ait olmalı.
    }


    public class Address
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

         public User User { get; set; }//user objesi oluşturdum.adresler tablosundaki her kayıt tek bir usere ait.bir user birine ait o yüzden user yukarıda list çünkü birden fazla adresi olabilir dedik
        public int UserId { get; set; }//eklemiş olduğum herhangi bir userin ıd bilgisini kullanarak gelip adres tablosuna bir kayırt ekleyebilirim
        //murlaka bir userin ıdsi var demek int? yaparsan nulda olsa sıkıntı olmaz//yabancı anahtar bu user
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }


    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Order
    {
        public int Id { get; set; }
        public int ProdutId { get; set; }
        public DateTime DateAdded { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //InsertUsers();
            //InsertAddresess();
            //navigation property yapıcaz
            //Navigation Properties: başka entity’ler ile olan ilişkiyi temsil eder. İki tipi vardır;
            using (var db = new ShopContext())
            {//user üzerinden adres ekleyeceğiz.İsmi şu olan kişi varsa şunları ekle

                var user = db.Users.FirstOrDefault(i => i.Username == "Ahmet Karadağ");
                if (user != null)
                {
                    user.Addresses = new List<Address>();//bunu yapmassak hata verir nul referance der bunu üretip sonra tamamız lazım bunun üzerine
                    user.Addresses.AddRange(//navigation property
                new List<Address>(){//addrange bu bilgileri liste halinde alacak
                new Address(){Fullname="Ahmet Karadağ",Title="Ev Adresi 1",Body="İstanbul",UserId=1},
                new Address(){Fullname="Ahmet Karadağ",Title="Ev Adresi 2",Body="İstanbul",UserId=1},
                new Address(){Fullname="Ahmet Karadağ",Title="Ev Adresi 3",Body="İstanbul",UserId=1}});
                    db.SaveChanges();
                }
            }
        }
        static void InsertUsers()
        {
            var users = new List<User>(){
                new User(){Username="Ahmet Karadağ",Email="ahmetikrdg@outlook.com"},
                new User(){Username="Yiğit Bilge",Email="yigitbilge@outlook.com"},
                new User(){Username="Ali Çelik",Email="celikali@outlook.com"},
                new User(){Username="Mehmet Güz",Email="cmet@outlook.com"}
            };
            using (var db = new ShopContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }
        }
        static void InsertAddresess()
        {
            var Adreses = new List<Address>(){
                new Address(){Fullname="Ahmet Karadağ",Title="Ev Adresi",Body="İstanbul",UserId=1},
                new Address(){Fullname="Yiğit Bilge",Title="İş Adresi",Body="İstanbul",UserId=2},
                new Address(){Fullname="Ali Çelik",Title="Ev Adresi",Body="İstanbul",UserId=3},
                                new Address(){Fullname="Ali Çelik",Title="İş Adresi",Body="İstanbul",UserId=3},
                                new Address(){Fullname="Mehmet Güz",Title="İş Adresi",Body="İstanbul",UserId=4},
                                new Address(){Fullname="Mehmet Güz",Title="Ev Adresi",Body="İstanbul",UserId=4}
            };
            using (var db = new ShopContext())
            {
                db.Addresses.AddRange(Adreses);
                db.SaveChanges();
            }
        }
    }
}





