import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule,FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, UrlMatcher, UrlMatchResult, UrlSegment } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { RedirectComponent } from './redirect/redirect.component';
import { NgxSpinnerModule } from "ngx-spinner";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ClipboardModule } from 'ngx-clipboard';

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        RedirectComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        NgxSpinnerModule,
        ClipboardModule,
        RouterModule.forRoot([
            {
                matcher: matchCustomUrl,
                component: RedirectComponent
            },
            { path: '', component: HomeComponent, pathMatch: 'full' }
        ])
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }

export function matchCustomUrl(url: UrlSegment[]): UrlMatchResult {
    if (url.length === 1 && url[0].path.match("^[a-z0-9]*$")) {
        return {
            consumed: url,
            posParams: {
                encodedurl: new UrlSegment(url[0].path, {})
            }
        };
    }
    return null;
}