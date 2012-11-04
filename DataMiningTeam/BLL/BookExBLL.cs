using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMiningTeam.Dto;
using DataMiningTeam.Data;
using System.Data;
using System.Data.SqlClient;

namespace DataMiningTeam.BLL
{
    class BookExBLL
    {
        public static List<TransactionDto> getAWSaleDto(){

            TransactionDto dto;
            List<TransactionDto> dtos = new List<TransactionDto>();

            using (AdventureWorksEntities awe = new AdventureWorksEntities())
            {
                IQueryable<BookExampleEntity> bee = awe.BookExampleEntities;

                foreach(BookExampleEntity be in bee)
                {
                    dto = new TransactionDto();
                    dto.tid = be.TID;
                    dto.items = be.items.Split('|').ToList();
                    dtos.Add(dto);
                }//foreach

            }//using

            return dtos;
        }

    }
}
