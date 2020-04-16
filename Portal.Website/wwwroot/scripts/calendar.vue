<template>
    <div>
        <navbar page="calendar"></navbar>
        <div class="container card">
            <FullCalendar :header="header"
                          :config="config"
                          :editable="editable"
                          :event-sources="eventSources"></FullCalendar>
        </div>
        <portalFooter></portalFooter>
    </div>
</template>

<script>
    import navbar from './components/navbar.vue';
    import portalFooter from './components/footer.vue';
    import { FullCalendar } from 'vue-full-calendar';
    import 'fullcalendar/dist/locale/nl';
    import { get } from './utils/communication';
    import 'bootstrap'; // required for the popover
    import Cookies from 'js-cookie';

    export default {
        data() {

            let locale = Cookies.get('locale');
            if (locale === null) {
                var lang = navigator.language || navigator.userLanguage;
                locale = lang[0] + lang[1];
            }
            console.log(locale);
            return {
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                editable: false,
                config: {
                    locale: locale,
                    buttonIcons: false, // show the prev/next text
                    weekNumbers: true,
                    eventBackgroundColor: '#ce8806',
                    eventBorderColor: '#ffffff',
                    themeSystem: 'bootstrap4',
                    themeButtonIcons: {
                        prev: 'circle-triangle-w',
                        next: 'circle-triangle-e',
                        prevYear: 'seek-prev',
                        nextYear: 'seek-next'
                    },
                    defaultView: 'month',
                    bootstrapFontAwesome: {
                        close: 'fa-times',
                        prev: 'fa-chevron-left',
                        next: 'fa-chevron-right',
                        prevYear: 'fa-angle-double-left',
                        nextYear: 'fa-angle-double-right'
                    },
                    eventRender: function (event, element) {
                        $(event.el).popover({
                            title: event.event.title,
                            html: true,
                            content: '<p>Start: ' + event.event.extendedProps.startStr + '</p><p> End: ' + event.event.extendedProps.endStr + '</p><div>' + event.event.extendedProps.information + '</div>',
                            trigger: 'hover',
                            placement: 'top',
                            container: 'body'
                        });
                    }
                },
                eventSources: [
                    {
                        events(startEnd, callback, failCallback) {
                            var start = encodeURIComponent(startEnd.startStr.substr(0, startEnd.startStr.length - 1));
                            var end = encodeURIComponent(startEnd.endStr.substr(0, startEnd.startStr.length - 1));
                            get('/api/calendar/getevents?start=' + start + '&end=' + end, callback);
                            
                        }
                    },
                ]

            };
        },
        components: {
            navbar,
            portalFooter,
            FullCalendar
        }
    }

</script>

<style lang="scss">
    @import '../assets/css/material-kit.css?v=2.0.5';
    @import '../node_modules/vue-full-calendar/node_modules/fullcalendar/dist/fullcalendar.css';
    @import '../styles/calendar.css';
</style>