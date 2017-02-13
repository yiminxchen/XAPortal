/// <binding BeforeBuild='inject-index' />
var gulp = require('gulp');
var inject = require('gulp-inject');
var concat = require('gulp-concat');
var print = require('gulp-print');
var angularFilesort = require('gulp-angular-filesort');
var paths = require('./gulp.config.json');

/**
 * Copy html template
 */
gulp.task('copy-htmltemplate', () => {
    console.log('preparing html template...');

    return gulp.src(paths.htmltemplates)
        .pipe(print())
        .pipe(gulp.dest(paths.root + 'template'));
});

/**
 * Copy vendor css 
 */
gulp.task('copy-css', () => {
    console.log('preparing vendor css....')

    return gulp.src(paths.vendorcss)
        .pipe(print())
        .pipe(concat('site.min.css'))
        .pipe(gulp.dest(paths.root + 'css'));
});

/**
 * Copy vendor js 
 */
gulp.task('copy-js', () => {
    console.log('preparing vendor js....')

    return gulp.src(paths.vendorjs)
        .pipe(print())
        //.pipe(angularFilesort())
        .pipe(concat('vendor.min.js'))
        .pipe(gulp.dest(paths.root + 'js'));
});

/**
 * Copy app js
 */
gulp.task('copy-appjs', () => {
    console.log('preparing app js....')

    return gulp.src(paths.appjs)
        .pipe(print())
        .pipe(concat('app.min.js'))
        .pipe(gulp.dest(paths.root + 'app'));
});

/**
 * build clietn - run all dependent tasks and then inject the index.html 
 */
gulp.task('build-client', ['copy-htmltemplate', 'copy-css', 'copy-js', 'copy-appjs'], () => {
    console.log('injecting index.html....')

    var appIndex = gulp.src(paths.src + 'index.html');
    var appjsrc = gulp.src(paths.root + 'app/*.js', { read: false });
    var csssrc = gulp.src(paths.root + 'css/site.min.css', { read: false });
    var jssrc = gulp.src(paths.root +'js/vendor.min.js', { read: false });

    return appIndex
        .pipe(inject(csssrc, { name: 'styles', ignorePath: paths.ignore }))
        .pipe(inject(jssrc, { name: 'vendorjs', ignorePath: paths.ignore }))
        .pipe(inject(appjsrc, { name: 'appjs', ignorePath: paths.ignore }))
        .pipe(gulp.dest(paths.root));
});