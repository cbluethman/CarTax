import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent
{
  zip: string
  income: number
  state: string
  formula: string
  tax: number
  monthly: boolean;

  constructor( private http: HttpClient ) { }

  onClick()
  {
    this.getState().subscribe(
      data => {
        this.state = (data as any).state

        this.getTax().subscribe(
          (data: TaxResult[]) => {
            this.formula = data[0].formula
            this.tax = data[0].tax
          },
          error => {
            console.log('Log the error here: ', error)
          })
      },
      error => {
        console.log('Log the error here: ', error)
      })
  }

  getTax(): Observable<TaxResult[]> {

    let income: number = (this.monthly == true) ? this.income * 12 : this.income

    return this.http.get<TaxResult[]>('https://localhost:44349/Tax?state=' + this.state + '&income=' +
      income, { responseType: 'json' })
  }

  getState(): Observable<ZipInfo[]>
  {
    return this.http.get<ZipInfo[]>('https://ziptasticapi.com/' + this.zip,
      { responseType: 'json' })
  }
}

class ZipInfo
{
  public country: string
  public state: string
  public city: string

  constructor(country: string, state: string, city: string)
  {
    this.country = country
    this.state = state
    this.city = city
  }
}

class TaxResult
{
  public formula: string
  public tax: number

  constructor(formula: string, tax: number)
  {
      this.formula = formula
      this.tax = tax
  }
}
