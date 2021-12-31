import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, ParamMap, RouterModule, UrlSegment } from '@angular/router';
import { environment } from '../../environments/environment';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
})
export class HomeComponent {
    private _http: HttpClient;
    private _shortenedUrl: ApiResponse;
    private _spinner: NgxSpinnerService;

    constructor(
        http: HttpClient,
        route: ActivatedRoute,
        spinner: NgxSpinnerService
    ) {
        this._http = http;
        this._spinner = spinner;
    }

    public updatedUrl: string;
    public originalUrl; string;

    GetShortenedUrl(url: string) {
        let params = new HttpParams().set("url", url);
        this._spinner.show();
        this._http.get<ApiResponse>(environment.apiUrl + 'api/url-shortener/shorten-url', { params: params }).subscribe(result => {
            this._spinner.hide();
            this._shortenedUrl = result;
            this.updatedUrl = this._shortenedUrl.url;
        }, error => console.error(error));
        return url;
    }
}

interface ApiResponse {
    url: string;
    httpStatusCode: string;
}

