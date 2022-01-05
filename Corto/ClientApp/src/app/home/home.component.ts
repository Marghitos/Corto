import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { environment } from '../../environments/environment';
import { NgxSpinnerService } from "ngx-spinner";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClipboardService } from 'ngx-clipboard';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
})
export class HomeComponent {
    private _http: HttpClient;
    private _spinner: NgxSpinnerService;
    form: FormGroup = new FormGroup({});
    public updatedUrl: string;
    private _clipboardService: ClipboardService;
    public isCommunicationError: boolean;

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
            url: ['', [Validators.required, Validators.pattern('(https://)([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?')]]
        });
        this._clipboardService = clipboardService;
        this.isCommunicationError = false;
    }

    get f() {
        return this.form.controls;
    }

    copyContent() {
        this._clipboardService.copyFromContent(this.updatedUrl)
    }

    onSubmit() {
        this._spinner.show();
        const httpOptions: {
            headers; observe;
        } = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            }),
            observe: 'response'
        };

        this._http.post<string>(environment.apiUrl + 'api/url-shortener/shorten-url', JSON.stringify(this.form.value.url), httpOptions).subscribe(result => {
            this._spinner.hide();
            this.updatedUrl = result["body"];
            this.isCommunicationError = false;
        }, error => {
            this._spinner.hide();
            this.isCommunicationError = true;
        });
    }
}