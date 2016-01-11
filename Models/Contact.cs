using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ContactManager.Models
{
    public class Contact
    {
        private static readonly Random Random = new Random();

        #region Properties
        private string _initials;
        public string Initials
        {
            get
            {
                if (_initials == string.Empty && FirstName != string.Empty && LastName != string.Empty)
                {
                    _initials = FirstName[0].ToString() + LastName[0].ToString();
                }
                return _initials;
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                if (_name == string.Empty && FirstName != string.Empty && LastName != string.Empty)
                {
                    _name = FirstName + " " + LastName;
                }
                return _name;
            }
        }
        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                _initials = string.Empty; // force to recalculate the value 
                _name = string.Empty; // force to recalculate the value 
            }
        }
        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                _initials = string.Empty; // force to recalculate the value 
                _name = string.Empty; // force to recalculate the value 
            }
        }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public string Biography { get; set; }
        public string Photo { get; set; }
        #endregion

        public Contact()
        {
            // default values for each property.
            _initials = string.Empty;
            _name = string.Empty;
            LastName = string.Empty;
            FirstName = string.Empty;
            Position = string.Empty;
            PhoneNumber = string.Empty;
            Biography = string.Empty;
            Photo = string.Empty;
        }
     
        #region Public Methods
        public static Contact GetNewContact()
        {
            return new Contact()
            {
                FirstName = GenerateFirstName(),
                LastName = GenerateLastName(),
                Biography = GetBiography(),
                PhoneNumber = GeneratePhoneNumber(),
                Position = GeneratePosition(),
                Photo = GeneratePhoto()
            };
        }
        public static ObservableCollection<Contact> GetContacts(int numberOfContacts)
        {
            ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();

            for (int i = 0; i < numberOfContacts; i++)
            {
                contacts.Add(GetNewContact());
            }
            return contacts;
        }
        public static ObservableCollection<GroupInfoList> GetContactsGrouped(int numberOfContacts)
        {
            ObservableCollection<GroupInfoList> groups = new ObservableCollection<GroupInfoList>();

            var query = from item in GetContacts(numberOfContacts)
                        group item by item.LastName[0] into g
                        orderby g.Key
                        select new { GroupName = g.Key, Items = g };

            foreach (var g in query)
            {
                GroupInfoList info = new GroupInfoList {Key = g.GroupName};
                info.AddRange(g.Items);
                groups.Add(info);
            }

            return groups;
        }
        #endregion

        #region Helpers
        private static string GeneratePosition()
        {
            List<string> positions = new List<string>() { "Chef de projet", "Developeur", "Architecte", "Consultant" };
            return positions[Random.Next(0, positions.Count)];
        }
        private static string GetBiography()
        {
            List<string> biographies = new List<string>()
            {
                @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id facilisis lectus. Cras nec convallis ante, quis pulvinar tellus. Integer dictum accumsan pulvinar. Pellentesque eget enim sodales sapien vestibulum consequat.",
                @"Maecenas eu sapien ac urna aliquam dictum.",
                @"Nullam eget mattis metus. Donec pharetra, tellus in mattis tincidunt, magna ipsum gravida nibh, vitae lobortis ante odio vel quam.",
                @"Quisque accumsan pretium ligula in faucibus. Mauris sollicitudin augue vitae lorem cursus condimentum quis ac mauris. Pellentesque quis turpis non nunc pretium sagittis. Nulla facilisi. Maecenas eu lectus ante. Proin eleifend vel lectus non tincidunt. Fusce condimentum luctus nisi, in elementum ante tincidunt nec.",
                @"Aenean in nisl at elit venenatis blandit ut vitae lectus. Praesent in sollicitudin nunc. Pellentesque justo augue, pretium at sem lacinia, scelerisque semper erat. Ut cursus tortor at metus lacinia dapibus.",
                @"Ut consequat magna luctus justo egestas vehicula. Integer pharetra risus libero, et posuere justo mattis et.",
                @"Proin malesuada, libero vitae aliquam venenatis, diam est faucibus felis, vitae efficitur erat nunc non mauris. Suspendisse at sodales erat.",
                @"Aenean vulputate, turpis non tincidunt ornare, metus est sagittis erat, id lobortis orci odio eget quam. Suspendisse ex purus, lobortis quis suscipit a, volutpat vitae turpis.",
                @"Duis facilisis, quam ut laoreet commodo, elit ex aliquet massa, non varius tellus lectus et nunc. Donec vitae risus ut ante pretium semper. Phasellus consectetur volutpat orci, eu dapibus turpis. Fusce varius sapien eu mattis pharetra.",
                @"Nam vulputate eu erat ornare blandit. Proin eget lacinia erat. Praesent nisl lectus, pretium eget leo et, dapibus dapibus velit. Integer at bibendum mi, et fringilla sem."
            };
            return biographies[Random.Next(0, biographies.Count)];
        }

        private static string GeneratePhoneNumber()
        {
            return string.Format("{0:(###)} {1:###}-{2:####}", Random.Next(100, 999), Random.Next(100, 999), Random.Next(1000, 9999));
        }
        private static string GenerateFirstName()
        {
            List<string> names = new List<string>() { "Antoine", "Jérome", "Sophie", "Maher", "Thibaut", "Thomas", "Mariane", "Alexandre", "Sara", "Arnaud", "Jean-Louis", "Jérémy", "Sandra", "Anne", "Julie", "Chloé", "Natalie", "Henrie", "Fanny", "Quentin", "Vianey", "Bastien", "Léo", "Jimmy", "Justine", "Kevin", "Elise", "Alice", "Sacha", "Damien", "Gregory", "Victor", "Victorien", "Jordan", "Matéo", "Arthur", "Fabrice", "Mariane", "Michel", "Lucas", "Nathan", "Lola", "Jean", "Franque", "Vincent", "René", "Adrien", "Alain", "Robert", "Antony", "Laurent", "Nicolas", "Mathieu", "Elly", "Carlos", "Diego", "Charles", "Xavier", "Elodie", "Eliane", "Roxane", "Amanda", "Ingrid", "Fernand", "Malika", "Raphael", "Jules", "Louis", "Andréa", "Hubert", "Frédérique", "Edoir", "Benoit", "Celine", "Brice", "Marcus", "Daniel", "Mathea", "Cézar", "Christian", "Claude", "Cirylle", "Constantin", "Correntin", "Didier", "Dorian", "David", "Daniel", "Esteban", "Eric", "Fabien", "Arnaud", "Florent", "Georges", "Gérard", "Gontran", "Guillaume", "Julien", "Justin", "Colette" };
            return names[Random.Next(0, names.Count)];
        }
        private static string GenerateLastName()
        {
            List<string> lastnames = new List<string>() { "Carlson", "Attia", "Quint", "Hollenberg", "Khoury", "Araujo", "Hakimi", "Seegers", "Abadi", "al", "Krommenhoek", "Siavashi", "Kvistad", "Sjo", "Vanderslik", "Fernandes", "Dehli", "Sheibani", "Laamers", "Batlouni", "Lyngvær", "Oveisi", "Veenhuizen", "Gardenier", "Siavashi", "Mutlu", "Karzai", "Mousavi", "Natsheh", "Seegers", "Nevland", "Lægreid", "Bishara", "Cunha", "Hotaki", "Kyvik", "Cardoso", "Pilskog", "Pennekamp", "Nuijten", "Bettar", "Borsboom", "Skistad", "Asef", "Sayegh", "Sousa", "Medeiros", "Kregel", "Shamoun", "Behzadi", "Kuzbari", "Ferreira", "Van", "Barros", "Fernandes", "Formo", "Nolet", "Shahrestaani", "Correla", "Amiri", "Sousa", "Fretheim", "Van", "Hamade", "Baba", "Mustafa", "Bishara", "Formo", "Hemmati", "Nader", "Hatami", "Natsheh", "Langen", "Maloof", "Berger", "Ostrem", "Bardsen", "Kramer", "Bekken", "Salcedo", "Holter", "Nader", "Bettar", "Georgsen", "Cunha", "Zardooz", "Araujo", "Batalha", "Antunes", "Vanderhoorn", "Nader", "Abadi", "Siavashi", "Montes", "Sherzai", "Vanderschans", "Neves", "Sarraf", "Kuiters" };
            return lastnames[Random.Next(0, lastnames.Count)];
        }


        private static string GeneratePhoto()
        {
            int id = Random.Next(1, 9);
            return $"../../Assets/Photos/{id}.jpg";
        }
        #endregion
    }
}
