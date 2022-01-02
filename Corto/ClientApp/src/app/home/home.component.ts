import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, ParamMap, RouterModule, UrlSegment } from '@angular/router';
import { environment } from '../../environments/environment';
import { NgxSpinnerService } from "ngx-spinner";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClipboardModule, ClipboardService } from 'ngx-clipboard';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
})
export class HomeComponent {
    private _http: HttpClient;
    private _shortenedUrl: ApiResponse;
    private _spinner: NgxSpinnerService;
    form: FormGroup = new FormGroup({});
    public updatedUrl: string;
    private _clipboardService: ClipboardService;

    constructor(
        http: HttpClient,
        route: ActivatedRoute,
        private formBuilder: FormBuilder,
        spinner: NgxSpinnerService,
        clipboardService: ClipboardService
    ) {
        this._http = http;
        this._spinner = spinner;
        this.form = formBuilder.group({
            url: ['', [Validators.required, Validators.pattern('(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?')]]
        });
        this._clipboardService = clipboardService;
    }

    get f() {
        return this.form.controls;
    }

    copyContent() {
        this._clipboardService.copyFromContent(this.updatedUrl)
    }

    onSubmit() {
        let params = new HttpParams().set("url", this.form.value.url);
        this._spinner.show();
        this._http.get<ApiResponse>(environment.apiUrl + 'api/url-shortener/shorten-url', { params: params }).subscribe(result => {
            this._spinner.hide();
            this._shortenedUrl = result;
            this.updatedUrl = this._shortenedUrl.url;
        }, error => console.error(error));
    }
}

interface ApiResponse {
    url: string;
    httpStatusCode: string;
}

