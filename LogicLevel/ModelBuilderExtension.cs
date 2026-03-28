using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectDataStructure.Addressrelatedclasses;
using ProjectDataStructure.Enum;
using ProjectDataStructure.Indiaclass.AutoMobile;
using System;

namespace LogicLevel
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                         new
                         {
                             Id = "823365c1-98fd-41b8-8b82-a4aa8f2b7901",
                             Name = "Admin",
                             NormalizedName = "ADMIN",
                             ConcurrencyStamp = "d3ce6efc-2d70-47cc-a599-4a9f9f8b9331",
                         },
                         new
                         {
                             Id = "91a2f26d-b11e-4f60-b249-517fc7c096a6",
                             Name = "User",
                             NormalizedName = "USER",
                             ConcurrencyStamp = "3d537b1f-f702-462a-bc76-7940ecac2935",
                         },
                         new
                         {
                             Id = "c0a93be2-a5e7-4116-97ba-1f01d875950c",
                             Name = "Second-Admin",
                             NormalizedName = "SECOND-ADMIN",
                             ConcurrencyStamp = "ffc62f96-c046-4646-a2cd-e5a88b3839e4"


                         },
                         new
                         {
                             Id = "c0a93be2-a5e7-4116-97ba-1f01d875952c",
                             Name = "Services Provider",
                             NormalizedName = "SERVICES PROVIDER",
                             ConcurrencyStamp = "900cc264-c620-47d2-ab1a-76a8da6c6eeb",
                         },
                          new
                          {
                              Id = "cf36f71b-1ff9-45ad-9b3f-636c53967b9c",
                              Name = "Super-Admin",
                              NormalizedName = "SUPER-ADMIN",
                              ConcurrencyStamp = "a30e3670-55f0-4ed4-bd35-7ad508552b95",
                          }
             );
            modelBuilder.Entity<ServicesType>().HasData(
            new ServicesType
            {
                ServicesTypeId = 1,
                ServiceType = "Plumbing"
            },
            new ServicesType
            {
                ServicesTypeId = 2,
                ServiceType = "Electricians",
            }
            );
            modelBuilder.Entity<ModelYear>().HasData(
                   new ModelYear
                   {
                       Id = 1,
                       year = 2005
                   },
                   new ModelYear
                   {

                       Id = 2,
                       year = 2006
                   },
                   new ModelYear
                   {
                       Id = 3,
                       year = 2007
                   },
                   new ModelYear
                   {
                       Id = 4,
                       year = 2008
                   },
                   new ModelYear
                   {
                       Id = 5,
                       year = 2009
                   },
                   new ModelYear
                   {
                       Id = 6,
                       year = 2010
                   },
                   new ModelYear
                   {
                       Id = 7,
                       year = 2011
                   },
                   new ModelYear
                   {
                       Id = 8,
                       year = 2012
                   },
                   new ModelYear
                   {
                       Id = 9,
                       year = 2013
                   },
                   new ModelYear
                   {
                       Id = 10,
                       year = 2014
                   },
                   new ModelYear
                   {
                       Id = 11,
                       year = 2015
                   },
                   new ModelYear
                   {
                       Id = 12,
                       year = 2016
                   },
                   new ModelYear
                   {
                       Id = 13,
                       year = 2017
                   },
                   new ModelYear
                   {
                       Id = 14,
                       year = 2018
                   },
                   new ModelYear
                   {
                       Id = 15,
                       year = 2019
                   }
                );
            modelBuilder.Entity<IndiaCity>().HasData(
                new IndiaCity
                {
                    CityId = 1,
                    CityName = "Ahmedabad",
                    indiastate = IndiaState.GJ,
                },
                new IndiaCity
                {
                    CityId = 2,
                    CityName = "Gandhinagar",
                    indiastate = IndiaState.GJ,
                },
                new IndiaCity
                {
                    CityId = 3,
                    CityName = "Chandkheda",
                    indiastate = IndiaState.GJ,
                },
                new IndiaCity
                {
                    CityId = 4,
                    CityName = "Pethapur",
                    indiastate = IndiaState.GJ,
                },
                new IndiaCity
                {
                    CityId = 5,
                    CityName = "Randheja",
                    indiastate = IndiaState.GJ,
                },
                new IndiaCity
                {
                    CityId = 6,
                    CityName = "Rupal",
                    indiastate = IndiaState.GJ,
                },
                new IndiaCity
                {
                    CityId = 7,
                    CityName = "Adalaj",
                    indiastate = IndiaState.GJ,
                },
                new IndiaCity
                {
                    CityId = 8,
                    CityName = "Kalol",
                    indiastate = IndiaState.GJ,
                }
                );

        }
    }
}
