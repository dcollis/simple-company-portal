<template>
    <nav id="mainnav" class="navbar navbar-expand-lg" v-bind:class="page == 'dashboard' ? 'navbar-color-on-scroll navbar-transparent fixed-top' : ''" color-on-scroll="100">
        <div class="container">
            <div class="navbar-translate">
                <a class="navbar-brand" href="/">
                    <img class="logo"/>
                </a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="navbar-toggler-icon"></span>
                    <span class="navbar-toggler-icon"></span>
                    <span class="navbar-toggler-icon"></span>
                </button>
            </div>

            <div class="collapse navbar-collapse">
                <ul class="navbar-nav ml-auto">
                    <li class="nav-item">
                        <a href="index.html" class="nav-link">
                            <i id="dashboardIcon" class="fa fa-grip-horizontal fa-2x" v-bind:class="page == 'dashboard' ? 'material-icons-active' : ''" data-toggle="tooltip" :title="$t('dashboard')"></i>
                            <span class="nav-label">Dashboard</span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <a href="news.html" class="nav-link">
                            <i id="newsIcon" class="far fa-newspaper fa-2x" v-bind:class="page == 'news' ? 'material-icons-active' : ''" data-toggle="tooltip" :title="$t('news')"></i>
                            <span class="nav-label">News</span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <a href="documents.html" class="nav-link">
                            <i id="documentsIcon" class="far fa-folder fa-2x" v-bind:class="page == 'documents' ? 'material-icons-active' : ''" data-toggle="tooltip" :title="$t('documents')"></i>
                            <span class="nav-label">Documents</span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <a href="calendar.html" class="nav-link">
                            <i id="calendarIcon" class="far fa-calendar-alt fa-2x" v-bind:class="page == 'calendar' ? 'material-icons-active' : ''" data-toggle="tooltip" :title="$t('calendar')"></i>
                            <span class="nav-label">Calendar</span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <a href="people.html" class="nav-link">
                            <i id="personIcon" class="far fa-address-book fa-2x" v-bind:class="page == 'people' ? 'material-icons-active' : ''" data-toggle="tooltip" :title="$t('people')"></i>
                            <span class="nav-label">People</span>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
</template>
<script>
    export default {
        name: 'mainnav',
        props: ['page']
    }
</script>

<style lang="scss">
    @import '../../assets/css/material-kit.css?v=2.0.5';
    @import '../../styles/main.css';
    @import url("https://use.fontawesome.com/releases/v5.7.2/css/all.css");

    .logo {
        content: url("/assets/img/merit-capital-logo.jpg");
        max-height: 60px;
        padding-bottom: 10px;
    }

    .navbar-transparent .container .navbar-translate .navbar-brand .logo {
        content: url("/assets/img/Merit-Capital-logo2.png");
        max-height: 100px
    }
</style>