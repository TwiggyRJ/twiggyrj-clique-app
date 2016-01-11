using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Newtonsoft.Json;

namespace Clique.DataModel
{
    public class Review
    {
        public string Reviewer { get; set; }

        public string UReview { get; set; }

        public DateTime Reviewed { get; set; }

        public string Avatar { get; set; }

        public string RID { get; set; }

        public string VID { get; set; }
    }
    public class Venue
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string Recommended { get; set; }

        public string Owner { get; set; }

        public string ID { get; set; }

        public string ImagePath { get; set; }

        

    }

    public class VenueCategory
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public List<Venue> Items { get; set; }
        public List<Review> Reviews { get; set; }

    }
    public class VenueDataSource
    {

        public List<VenueCategory> Items { get; set; }
        public List<VenueCategory> Reviews { get; set; }


        public VenueDataSource(string JSON)
        {
            List<Venue> venues = JsonConvert.DeserializeObject<List<Venue>>(JSON);

            var venuesByCategories = venues.GroupBy(x => x.Type)
                                .Select(x => new VenueCategory { Title = x.Key, Items = x.ToList() });

            Items = venuesByCategories.ToList();


        }
        public VenueDataSource(string JSON, string JSONReview)
        {
            List<Venue> venues = JsonConvert.DeserializeObject<List<Venue>>(JSON);
            List<Review> reviews = JsonConvert.DeserializeObject<List<Review>>(JSONReview);

            var venuesByCategories = venues.GroupBy(x => x.Type)
                                .Select(x => new VenueCategory { Title = x.Key, Items = x.ToList() });

            Items = venuesByCategories.ToList();

            if(JSONReview != "")
            {
                var reviewsByAuthor = reviews.GroupBy(x => x.Reviewer)
                                .Select(x => new VenueCategory { Author = x.Key, Reviews = x.ToList() });

                Reviews = reviewsByAuthor.ToList();
            }

        }
    }

}
