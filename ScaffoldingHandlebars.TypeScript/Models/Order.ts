﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
import { Customer } from './Customer';
import { OrderDetail } from './OrderDetail';

export interface Order {
    orderId: number;
    customerId: string;
    orderDate: Date;
    shippedDate: Date;
    shipVia: number;
    freight: number;
    customer: Customer;
    orderDetail: OrderDetail[];
}