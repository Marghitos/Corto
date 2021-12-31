import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, ParamMap, RouterModule, UrlSegment } from '@angular/router';
import { HttpStatusCode } from '../../enum/http-status-code';
import { environment } from '../../environments/environment';

@Component({
    selector: 'app-redirect',
    templateUrl: './redirect.component.html',
})

export class RedirectComponent implements OnInit {
    private _http: HttpClient;
    private _shortenedUrl: ClientResponse;
    private _route: ActivatedRoute;

    public RedirectStatus: string;

    constructor(
        http: HttpClient,
        route: ActivatedRoute
    ) {
        this._http = http;
        this._route = route;
    }

    ngOnInit(): void {
        this._route.paramMap.subscribe((params: ParamMap) => {
            var encodedUrl = params.get('encodedurl');
            this.GetDecodedUrl(encodedUrl);
        });
    }

    ApiResponseToClientResponse(apiResponse: ApiResponse): ClientResponse {
        return {
            url: apiResponse.url,
            httpStatusCode: this.MapHttpStatusCode(apiResponse.httpStatusCode)
        }
    }

    MapHttpStatusCode(httpStatusCode: number): HttpStatusCode {
        switch (httpStatusCode) {
            case 200:
                return HttpStatusCode.Ok;
            case 400:
                return HttpStatusCode.BadRequest;
            case 404:
                return HttpStatusCode.NotFound;
            case 410:
                return HttpStatusCode.Gone;
            default:
                return HttpStatusCode.Undefined;
        }
    }

    GetDecodedUrl(url: string) {
        let params = new HttpParams().set("url", url);
        this._http.get<ApiResponse>(environment.apiUrl + 'api/url-shortener/expand-url', { params: params }).subscribe(result => {
            this._shortenedUrl = this.ApiResponseToClientResponse(result);
            switch (this._shortenedUrl.httpStatusCode) {
                case HttpStatusCode.Ok:
                    this.RedirectStatus = "Redirecting to.. " + this._shortenedUrl.url;
                    window.location.href = this._shortenedUrl.url;
                    break;
                case HttpStatusCode.BadRequest:
                    this.RedirectStatus = "There was an error. Please try again later.";
                    break;
                case HttpStatusCode.NotFound:
                    this.RedirectStatus = "Url not found. Please create a new one.";
                    break;
                case HttpStatusCode.Gone:
                    this.RedirectStatus = "Url is expired. Please create a new one.";
                    break;
            }
        }, error => console.error(error));
    }
}

interface ApiResponse {
    url: string;
    httpStatusCode: number;
}

interface ClientResponse {
    url: string;
    httpStatusCode: HttpStatusCode;
}

