import { Component, OnInit } from '@angular/core';
import { AuthService } from '../service/auth.service';
import { Router } from '@angular/router';
import { Item } from '../models/item.model';
import { ApiServiceService } from '../service/api-service.service';
import Swal from 'sweetalert2';

interface Stock {
  itemid: number;
  itemname: string;
  data_list: Array<{ center: string; stock: number }>;
  subtotal: number;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  itemList: Item[] = [];
  currentItemList: Item[] = [];
  HighlightRow: number = -1;
  currentItem: Item | null = null;
  selectedItemIndex: number = -1;
  stockList: Stock[] = [];
  icd: any = "";
  icj: any = "";
  scl: any = "";
  isb: any = "";
  isj: any = "";
  icdTotal: number = 0;
  icjTotal: number = 0;
  sclTotal: number = 0;
  isbTotal: number = 0;
  isjTotal: number = 0;
  totalCash: number = 0;
  totalCard: number = 0;
  totalCredit: number = 0;
  totalEmi: number = 0;
  totalMobile: number = 0;
  totalEft: number = 0;
  totalCheque: number = 0;
  totalOnline: number = 0;
  loading = false;
  showSalesReport: boolean = false;
  showItems: boolean = true;
  showItemReport: string = "Hide";
  showSalesReportMsg: string = "Show";
  salesReportMessage: any = "Show Today's Sales"
  showStockList = false;
  anyItemOnStock = false;
  
  constructor(
    private authService: AuthService,
    private router: Router,
    private action: ApiServiceService
  ) {}

  ngOnInit() {
    this.getItemMaster();
    this.getTodaysSale();
  }

  getItemMaster() {
    this.action.getItemMaster().subscribe(
      (res) => {
        this.itemList = res.sort((a, b) => a.itemname.localeCompare(b.itemname));
        this.currentItemList = this.itemList
          .filter((item) => item.parent === 0)
          .sort((a, b) => a.itemname.localeCompare(b.itemname));
      },
      (error) => {
        Swal.fire('Error', error?.error?.message, 'error');
      }
    );
    this.showStockList = false
  }

  onItemClick(newItem: Item, index: number) {
    const children = this.itemList.filter(
      (data) => data.parent === newItem.itemid
    );

    if (children.length === 0) {
      console.log('Leaf node clicked:', newItem);
      this.getStockUpdates(newItem.itemid);
    } else {
      this.getStockUpdates(newItem.itemid);
      console.log('Parent node clicked:', newItem);
      this.currentItemList = children.sort((a, b) => a.itemname.localeCompare(b.itemname));
      this.currentItem = newItem;
      this.selectedItemIndex = index;
    }
    this.showStockList= true;
  }

  onBack() {
    if (this.currentItem && this.currentItem.parent === 0) {
      this.currentItem = null;
      this.currentItemList = this.itemList.filter((item) => item.parent === 0);
      this.HighlightRow = -1;
    } else {
      if (this.currentItem) {
        const parentID = this.currentItem.parent;
        this.currentItem =
          this.itemList.find((item) => item.itemid === parentID) || null;
        this.currentItemList = this.itemList.filter(
          (item) => item.parent === parentID
        ).sort((a, b) => a.itemname.localeCompare(b.itemname));
      }
    }
    this.showStockList = false
    this.showItems = true;
    this.showItemReport = this.showItems ? 'Hide' : 'Show';
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  getStockUpdates(itemid: number = 10000000) {
    this.loading = true;
    this.action.getStocks(itemid).subscribe(
      (res) => {
        this.stockList = res.payload;
        this.anyItemOnStock= res.payload !== null;
        // if (this.stockList.length > 0 || res.payload !== null) {
        //   this.anyItemOnStock = true
        // }else {
        //   this.anyItemOnStock = false
        // }
        console.log("Is there any item? :", res.payload);
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.anyItemOnStock = false;
        Swal.fire('Error', error?.error?.message, 'error');
      }
    );
  }

  toggleItemShow() {
    this.showItems = !this.showItems;
    this.showItemReport = this.showItems ? 'Hide' : 'Show';
  }

  toggleSalesReport() {
    this.showSalesReport = !this.showSalesReport;
    this.showSalesReportMsg = this.showSalesReport ? 'Hide' : 'Show';
  }
  branches = [
  { key: 'icd', label: 'iCenter Dhanmondi' },
  { key: 'icj', label: 'iCenter Jamuna' },
  { key: 'scl', label: 'Satcom Computers' },
  { key: 'isb', label: 'iService Bashundhora' },
  { key: 'isj', label: 'iService Jamuna' },
];
sales: any = {};

  getTodaysSale() {
      this.action.getSaleDetails().subscribe(
        (res) => {
          this.icd = res.payload.icd;
          this.icj = res.payload.icj;
          this.scl = res.payload.scl;
          this.isb = res.payload.isb;
          this.isj = res.payload.isj;
          this.icdTotal = res.payload.icd.total_amount; //this.icd.daily_cashamt + this.icd.daily_cardamt + this.icd.daily_creditamt + this.icd.daily_emiamt + this.icd.daily_mobamt + this.icd.daily_eftamt + this.icd.daily_chequeamt + this.icd.daily_onlineamt
          this.icjTotal = res.payload.icj.total_amount; //this.icj.daily_cashamt + this.icj.daily_cardamt + this.icj.daily_creditamt + this.icj.daily_emiamt + this.icj.daily_mobamt + this.icj.daily_eftamt + this.icj.daily_chequeamt + this.icj.daily_onlineamt 
          this.sclTotal = res.payload.scl.total_amount; //this.scl.daily_cashamt + this.scl.daily_cardamt + this.scl.daily_creditamt + this.scl.daily_emiamt + this.scl.daily_mobamt + this.scl.daily_eftamt + this.scl.daily_chequeamt + this.scl.daily_onlineamt
          this.isbTotal = res.payload.isb.total_amount; //this.isb.daily_cashamt + this.isb.daily_cardamt + this.isb.daily_creditamt + this.isb.daily_emiamt + this.isb.daily_mobamt + this.isb.daily_eftamt + this.isb.daily_chequeamt + this.isb.daily_onlineamt 
          this.isjTotal = res.payload.isj.total_amount; //this.isj.daily_cashamt + this.isj.daily_cardamt + this.isj.daily_creditamt + this.isj.daily_emiamt + this.isj.daily_mobamt + this.isj.daily_eftamt + this.isj.daily_chequeamt + this.isj.daily_onlineamt 
          this.totalCash = this.icd.daily_cashamt + this.icj.daily_cashamt + this.scl.daily_cashamt + this.isb.daily_cashamt + this.isj.daily_cashamt;
          this.totalCard = this.icd.daily_cardamt + this.icj.daily_cardamt + this.scl.daily_cardamt + this.isb.daily_cardamt + this.isj.daily_cardamt;
          this.totalCredit = this.icd.daily_creditamt + this.icj.daily_creditamt + this.scl.daily_creditamt + this.isb.daily_creditamt + this.isj.daily_creditamt;
          this.totalEmi = this.icd.daily_emiamt + this.icj.daily_emiamt + this.scl.daily_emiamt + this.isb.daily_emiamt + this.isj.daily_emiamt;
          this.totalMobile = this.icd.daily_mobamt + this.icj.daily_mobamt + this.scl.daily_mobamt + this.isb.daily_mobamt + this.isj.daily_mobamt;
          this.totalEft = this.icd.daily_eftamt + this.icj.daily_eftamt + this.scl.daily_eftamt + this.isb.daily_eftamt + this.isj.daily_eftamt;
          this.totalCheque = this.icd.daily_chequeamt + this.icj.daily_chequeamt + this.scl.daily_chequeamt + this.isb.daily_chequeamt + this.isj.daily_chequeamt;
          this.totalOnline = this.icd.daily_onlineamt + this.icj.daily_onlineamt + this.scl.daily_onlineamt + this.isb.daily_onlineamt + this.isj.daily_onlineamt;
        },
        (error) => {
          Swal.fire('Error', error?.error?.message, 'error');
        }
      );
  }
}
