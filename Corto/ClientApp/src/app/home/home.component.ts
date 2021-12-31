import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, ParamMap, RouterModule, UrlSegment } from '@angular/router';
import { environment } from '../../environments/environment';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
})
export class HomeComponent {
    private _http: HttpClient;
    private _baseUrl: string;
    private _shortenedUrl: ApiResponse;    

    constructor(
        http: HttpClient,
        @Inject('BASE_URL') baseUrl: string,
        route: ActivatedRoute
    ) {
        this._http = http;
        this._baseUrl = baseUrl;
    }

    public updatedUrl: string;
    public originalUrl; string;

    GetShortenedUrl(url: string) {
        let params = new HttpParams().set("url", url);
        this._http.get<ApiResponse>(environment.apiUrl + 'api/url-shortener/shorten-url', { params: params }).subscribe(result => {
            this._shortenedUrl = result;
            this.updatedUrl = this._baseUrl + this._shortenedUrl.url;
        }, error => console.error(error));
        return url;
    }
}

interface ApiResponse {
    url: string;
    httpStatusCode: string;
}

