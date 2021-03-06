﻿using MarketServerTest.Helpers;

namespace Common.Models
{
    public class OrderBook
    {
        private double _price;
        private double _qantity;
        private string _type;

        public double Price
        {
            get => _price;
            set
            {
                _price = value; 
            }
        }

        public double Quantity
        {
            get => _qantity;
            set
            {
                _qantity = value;
            } 
        }
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
            } 
        }
    }
}
