﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApi.Models
{
    public class TradeHistory
    {
        [Key]
        public int historyId { get; set; }

        [Required]
        public DateTime createdDate { get; set; }

        [Required, MaxLength(20)]
        public string status { get; set; }  // created, updated, viewed, closed, removed

        public int tradeId { get; set; }

        public Trade Trade { get; set; }
      
        [Required, MaxLength(20)]
        public string viewer { get; set; }

        public TradeHistory() { }


        public TradeHistory(int hisId, int trdId, DateTime crda, string sta, string view)
        {
            historyId = hisId;
            tradeId = trdId;
            createdDate = crda;
            status = sta;
            viewer = view;
        }
    }




    public class TradeHistoryDTO
    {       
        public int historyId { get; set; }

        public DateTime createdDate { get; set; }

        public string status { get; set; }  // created, updated, viewed, closed, removed

        public int tradeId { get; set; }

        [Required, MaxLength(20)]
        public string viewer { get; set; }

        public TradeHistoryDTO() { }

        public TradeHistoryDTO(int hisId, int trdId, DateTime crda, string sta, string view)
        {
            historyId = hisId;
            tradeId = trdId;
            createdDate = crda;
            status = sta;
            viewer = view;
        }
    }

}