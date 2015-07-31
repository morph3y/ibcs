using System;
using Entities;

namespace Contracts.Business
{
    public class RankModel
    {
        public Subject Subject { get; set; }
        public int Elo { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}