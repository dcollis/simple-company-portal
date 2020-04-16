<template>
    <div>
        <navbar page="people"></navbar>
        <div class="container table-responsive card" id="peopleList">
            <div class="input-group">
                <div class="input-group-prepend">
                    <span class="input-group-text">
                        <i class="material-icons">group</i>
                    </span>
                </div>
                <input type="text" class="form-control" placeholder="Search" v-on:keyup="filter()" v-model="searchFilter">
            </div>
            <br />
            <div id="people-table">
                <table class="table">
                    <thead>
                        <tr class="meritColor" style="color: whitesmoke">
                            <th scope="col"></th>
                            <th scope="col">{{ $t("forename") }}</th>
                            <th scope="col">{{ $t("surname") }}</th>
                            <th scope="col">{{ $t("company") }}</th>
                            <th scope="col">{{ $t("phone") }}</th>
                            <th scope="col">{{ $t("email") }}</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-if="loading">
                            <td colspan="5">
                                <loadingWidget></loadingWidget>
                            </td>
                        </tr>
                        <tr v-for="(person, index) in people" data-toggle="modal" data-target="#loginModal" v-on:click="updateSelected(index)">
                            <td class="profile-image">
                                <div class="btn btn-primary btn-fab btn-round meritColor">
                                    <img :src="person.profilephoto" style="width: 50px;" v-if="person.hasPhoto" @error="showDefaultIcon(person)" />
                                    <i class="material-icons" v-if="!person.hasPhoto">person</i>
                                </div>
                            </td>
                            <td class="profile-info">{{ person.forename }}</td>
                            <td class="profile-info">{{ person.surname }}</td>
                            <td class="profile-info">{{ person.companyName }}</td>
                            <td v-if="person.directPhone != null && person.directPhone != ''" class="profile-info">{{ person.directPhone }}</td>
                            <td v-else-if="person.mobilePhone != null && person.mobilePhone != ''" class="profile-info">{{ person.mobilePhone }}</td>
                            <td v-else class="profile-info"></td>
                            <td class="profile-info">{{ person.emailAddress }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="modal fade" id="loginModal" tabindex="-1" role="">
                <div class="modal-dialog modal-login" role="document">
                    <div class="modal-content">
                        <div class="card card-signup card-plain">
                            <div class="modal-header">
                                <div class="card-header card-header-primary text-center meritGradientBackground">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                                        <i class="material-icons">clear</i>
                                    </button>

                                    <h4 class="card-title">
                                        <br />{{ selectedPerson.forename }} {{ selectedPerson.surname }}<br />
                                    </h4>
                                    <h6 class="card-title">{{ selectedPerson.companyName }}</h6>

                                </div>
                            </div>
                            <div class="modal-body">
                                <div class="card-body">
                                    <div><img :src="selectedPerson.profilephoto" style="width: 200px;" v-if="selectedPerson.hasPhoto" /></div>
                                    <div class="form-group bmd-form-group" v-if="selectedPerson.jobTitle != null">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text"><i class="material-icons">work</i></div>
                                            </div>
                                            <div>{{ selectedPerson.jobTitle }}</div>
                                        </div>
                                    </div>
                                    <div class="form-group bmd-form-group" v-if="selectedPerson.emailAddress != null">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text"><i class="material-icons">email</i></div>
                                            </div>
                                            <div><a :href="selectedPerson.mailableAddress">{{ selectedPerson.emailAddress }}</a></div>
                                        </div>
                                    </div>

                                    <div class="form-group bmd-form-group" v-if="selectedPerson.directPhone != null && selectedPerson.directPhone != ''">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text"><i class="material-icons">local_phone</i></div>
                                            </div>
                                            <div><a :href="selectedPerson.directPhonable">{{ selectedPerson.directPhone }}</a></div>
                                        </div>
                                    </div>
                                    <div class="form-group bmd-form-group" v-if="selectedPerson.mobilePhone != null && selectedPerson.mobilePhone != ''">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text"><i class="material-icons">smartphone</i></div>
                                            </div>
                                            <div><a :href="selectedPerson.mobilePhonable">{{ selectedPerson.mobilePhone }}</a></div>
                                        </div>
                                    </div>
                                    <div class="form-group bmd-form-group" v-if="selectedPerson.location != null">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text"><i class="material-icons">location_city</i></div>
                                            </div>
                                            <div>{{ selectedPerson.location }}</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <portalFooter></portalFooter>
    </div>
</template>

<script>
    import navbar from './components/navbar.vue';
    import portalFooter from './components/footer.vue';
    import loadingWidget from './components/loading.vue';

    export default {
        data() {
            this.init();
            window.onscroll = () => {

                    let bottomOfWindow = document.documentElement.scrollTop + window.innerHeight === document.documentElement.offsetHeight;

                    if (bottomOfWindow) {
                        this.loadMore();
                    }
                };
            return {
                searchFilter: '',
                selectedPerson: {
                    forename: '',
                    surname: '',
                    directPhone: '',
                    mobilePhone: '',
                    emailAddress: '',
                    mailableAddress: '',
                    profilephoto: '',
                    hasPhoto: true
                },
                totalNumPages: 10,
                currentPage: 1,
                people: [],
                loading: true,
                nextLink: ''
            }
        },
        methods: {
            init: function () {
                var self = this;
                self.loading = true;
                $.getJSON('api/people/getpeople',
                    function (response) {
                        response.users.forEach(function (element) {
                            element.profilephoto = 'api/people/profilephoto?emailAddress=' + element.emailAddress;
                            element.hasPhoto = true;
                        });
                        self.people = response.users;
                        self.nextLink = response.nextLink;
                        self.loading = false;
                    });
            },
            filter: function () {
                var self = this;
                if (self.searchFilter == '') {
                    self.init();
                } else {
                    $.getJSON('api/people/getfilteredpeople?filter=' + self.searchFilter,
                        function (response) {
                            response.users.forEach(function (element) {
                                element.profilephoto = 'api/people/profilephoto?emailAddress=' + element.emailAddress;
                                element.hasPhoto = true;
                            });
                            self.people = response.users;
                            self.nextLink = response.nextLink;
                        });
                }
            },
            updateSelected: function (index) {
                this.selectedPerson = this.people[index];
                this.selectedPerson.directPhonable = 'tel:' + this.selectedPerson.directPhone;
                this.selectedPerson.mobilePhonable = 'tel:' + this.selectedPerson.mobilePhone;
                this.selectedPerson.mailableAddress = 'mailto: ' + this.selectedPerson.emailAddress;
            },
            showDefaultIcon: function (person) {
                person.hasPhoto = false;
            },
            loadMore: function () {
                var self = this;
                if (self.nextLink == '') {
                    return;
                }
                var url = encodeURIComponent(self.nextLink);
                $.getJSON('api/people/getmorepeople?url='+ url,
                    function (response) {
                        response.users.forEach(function (element) {
                            element.profilephoto = 'api/people/profilephoto?emailAddress=' + element.emailAddress;
                            element.hasPhoto = true;
                        });
                        self.people.push(...response.users);
                        self.nextLink = response.nextLink;
                    });
            }
        },
        components: {
            navbar,
            portalFooter,
            loadingWidget
        }
    }

</script>


<style lang="scss">
    @import url("https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons");

    th {
        text-transform: capitalize;
    }

    .meritColor {
        background-color: #24145d !important;
        border-color: #24145d !important;
    }

    .meritGradientBackground {
        background: linear-gradient(60deg, #4c32ab, #24145d) !important;
    }
</style>