<template>
    <div class="card">
        <div class="card-body" id="calendar">
            <h4 class="card-title">{{ $t("calendar") }}</h4>
            <h6 class="card-subtitle mb-2 text-muted">{{ $t("upcoming events") }}</h6>

            <div>
                <loadingAnimation v-if="loading"></loadingAnimation>
                <ul v-if="!loading" class="list-group list-group-flush" v-for="cal in calendarItems">
                    <li class="list-group-item">
                        <div class="row" style="width: 100%">
                            <div class="col-md-1">
                                <div class="row capitalize">
                                    {{ cal.date | moment("ddd")}}
                                </div>
                                <div class="row capitalize">
                                    {{ cal.date | moment("Do")}}
                                </div>
                                <div class="row capitalize">
                                    {{ cal.date | moment("MMMM")}}
                                </div>
                            </div>
                            <div class="col-md">
                                <div class="row calendar-event capitalize" v-for="event in cal.events" :id="event.id">
                                    <a style="width: 300px">{{ event.title }}</a>
                                    <br/>
                                    <b-popover :target="event.id"
                                               placement="top"
                                               :title="event.title"
                                               triggers="hover focus">
                                        <template>
                                            <p v-if="!event.allDay">{{ event.start | moment("hh:mm a") }} - {{ event.end | moment("hh:mm a") }}</p>
                                            <p v-if="event.allDay">{{ $t('All day') }}</p>
                                            <div>{{ event.information }}</div>
                                        </template>
                                    </b-popover>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>

<script>
    import loadingAnimation from '../../loading.vue';
    import { get } from '../../../utils/communication';
    import popover from 'bootstrap-vue/esm/components/popover/popover';
    import popoverDirective from 'bootstrap-vue/esm/directives/popover/popover';

    export default {
        data() {
            return {
                calendarItems: [],
                loading: true
            }
        },
        created() {
            this.getCalendarItems();
        },
        methods: {
            getCalendarItems: function () {
                var self = this;
                self.loading = true;
                get(
                    'api/calendar/getnextevents?numevents=5',
                    function (response) {
                        console.log(response);
                        self.calendarItems = response;
                        self.loading = false;
                    });
            }
        },
        components: {
            loadingAnimation,
            'b-popover': popover
        },
        directives: {
            'b-popover': popoverDirective
        }
    }
</script>