# coding: utf-8

"""
    Oppdateringsgrensesnitt for SFKB

    # NGIS-OpenAPI  Grov oversikt over funksjonalitet:   - Hente liste over tilgjengelige datasett    - Hente metadata for et bestemt datasett   - Hente data fra et bestemt datasett     - Med lesetilgang eller skrivetilgang (medfører låsing)       - områdebegrensning       - egenskapsspørring (begrenset i første versjon til bygningsnummer eller lokalid)   - Lagre data til et bestemt datasett     - Operasjoner som håndteres: nytt objekt, endre objekt og slett objekt  ## Generelle prinsipper for systemet  ### Versjonering og bakoverkompatibilitet  #### Versjonsnummer i URL  Vi har et versjonsnummer `v1` i URL for å gjøre det mulig å gjøre store endringer i APIet hvis det blir nødvendig, men i utgangspunktet ønsker vi å unngå å endre dette versjonsnummeret.  #### Versjonsnummer i media types (\"media type versioning\", content negotiation)  APIet baserer seg på standard [HTTP content negotiation](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept) ved utveksling av data med headerne `Accept` og `Content-Type`. Dette gjør det veldig enkelt å introdusere nye dataformater i APIet uten endringer for eksisterende klienter. I tillegg til dette inneholder også alle dataformater et versjonsparameter, eks. `version=1.0`, der klienten kan styre hvilket  eller hvilke dataformater klienten kan håndtere. Dataformater angitt uten versjonsparameter vil tolkes som å be om siste versjon.  `Accept: application/vnd.kartverket.sosi+json; version=1.0` Klienten ønsker svar med versjon 1 av dataformatet      `Accept: application/vnd.kartverket.sosi+json; version=2.0` Klienten ønsker svar med versjon 2 av dataformatet  `Accept: application/vnd.kartverket.sosi+json` Klienten ønsker svar med siste versjon av dataformatet  `Accept: application/vnd.kartverket.sosi+json; version=1.0, application/vnd.kartverket.sosi+json; version=2.0` Klienten håndterer både versjon 1.0 og 2.0 av dataformatet, og gir derfor tjenesten mulighet til å velge hvilken som skal brukes  `Accept: */*` Klienten håndterer alle dataformater, og gir derfor tjenesten mulighet til å velge hvilken som skal brukes.  Der man kan velge mellom flere helt ulike dataformater som f.eks GML og JSON, må man faktisk håndtere begge.  ### Delt geometri  Flater består av avgrensningslinjer som ligger lagret som egne objekter. På den måten kan en linje avgrense ingen, én eller flere flater. Det er likevel slik at flater hentes ut og lagres med egen geometri for å gjøre det enklere å tegne opp datene, men ved endring av (delte) linjer og flater må det tas hensyn til delt geometri. Forsøk på endring av linje eller flate uten tilsvarende endring av evt. delt geometri vil bli avvist av systemet.  ### Låsing  Dette er nærmere beskrevet i de aktuelle kallene.  Foreløpig er det kun `user_lock` som er støttet. Det betyr at data må hentes ut med `user_lock` før de kan sendes inn med endringer.  ### Porsjonering  All uthenting av feature-objekter vil kunne bli porsjonert av serveren, se `limit`-parameteret.   ### Koordinatsystemer og transformasjon  Dersom annet koordinatsystem enn det som ligger i dataset skal brukes (se `GET /datasets/{datasetId}`) må koordinatsystem angis med `crs_EPSG`-parameteret. Dette styrer data som sendes inn, data som hentes ut og koordinatsystemet i `bbox`-parameteret i kallet. For å bytte rekkefølge på aksene brukes `crs_normalized_for_visualization`-parameteret.  ### Historikk og historiske endringer  Det er mulig å hente ut data for et gitt tidspunkt (for hele datasettet eller begrenset til et område, et objekt etc.). Se etter parameteret `dataset_at`.  Det er også mulig å hente ut historikken som endringer, f.eks som endringer fra et tidligere uthentet område, objekt eller helt dataset. Se etter parameteret `dataset_modified`.  ### FKB5 og QMS13  I forbindelse med FKB5 har det blitt gjort endringer i GeoJSON-formatet som benyttes i NGIS-OpenAPI. Endringene gjelder ved bruk av Versjon 2 av formatet,  som er påkrevd versjon for FKB 5.  Endringer i GeoJSON-formatet, Versjon 2:   - Noden `geometry_properties` er flyttet under `properties`   - Formatet støtter assosiasjoner mellom objekter og geometri-assosiasjoner ved flater med delt geometri   - Formatet støtter heleid geometri  #### Assosiasjoner mellom objekter Et objekt kan ha en assosiasjon til ett eller flere andre objekter. Assosiasjonene av samme type ligger i en array med `lokalId` og `featuretype` til det assosierte objektet.  Flater kan være modellert med krav om delt geometri for flateavgrensningen. Dette angis via geometri-assosiasjoner fra flate-objektet til linjene-objektene som avgrenser flata.  I tillegg til assosiasjons-informasjonen beskrevet over, har geometri-assosiasjonene følgende egenskaper for hvert assosiert objekt:      `reverse` er en bool som forteller om linjas retning skal snus eller ikke for å danne en sammenhengende flateavgrensing med de andre avgrensningslinjene.  `idx` er ett array med tre indekser som gir informasjon om avgrensningslinja:    1. angir hvilken flate linja tilhører (aktuell ved framtidig bruk av MultiSurface), skal for enkeltflater være 0    2. angir om linja tilhører ytre eller en indre avgrensning (hull) og eventuelt hvilken indre avgrensning linja tilhører       - Ytre avgrensning: 0       - Indre avgrensning/hull: 1..n    3. angir hvilken rekkefølge linja har i avgrensningen av flata som dannes av alle avgrensninslinjene (starter på 0)   # noqa: E501

    OpenAPI spec version: 1.0.0
    
    Generated by: https://github.com/swagger-api/swagger-codegen.git
"""

from __future__ import absolute_import

import re  # noqa: F401

# python 2 and python 3 compatibility library
import six

from swagger_client.api_client import ApiClient


class FeaturesApi(object):
    """NOTE: This class is auto generated by the swagger code generator program.

    Do not edit the class manually.
    Ref: https://github.com/swagger-api/swagger-codegen
    """

    def __init__(self, api_client=None):
        if api_client is None:
            api_client = ApiClient()
        self.api_client = api_client

    def get_dataset_feature(self, x_client_product_version, dataset_id, lokal_id, references, **kwargs):  # noqa: E501
        """Hent ut en bestemt feature fra et dataset  # noqa: E501

        Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.  # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.get_dataset_feature(x_client_product_version, dataset_id, lokal_id, references, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str lokal_id: Identifikasjon.Lokalid til objektet (required)
        :param str references: Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  `none` - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  `direct` - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  `all` - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra `direct` også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i `direct`. Både flaten og alle avgrensningskurvene låses.  (required)
        :param str locking_type: Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes. 
        :param int limit: Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med `rel=next` i [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med `rel=next`. 
        :param int crs_epsg: Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes. 
        :param bool normalized_for_visualization: Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden. 
        :return: object
                 If the method is called asynchronously,
                 returns the request thread.
        """
        kwargs['_return_http_data_only'] = True
        if kwargs.get('async_req'):
            return self.get_dataset_feature_with_http_info(x_client_product_version, dataset_id, lokal_id, references, **kwargs)  # noqa: E501
        else:
            (data) = self.get_dataset_feature_with_http_info(x_client_product_version, dataset_id, lokal_id, references, **kwargs)  # noqa: E501
            return data

    def get_dataset_feature_with_http_info(self, x_client_product_version, dataset_id, lokal_id, references, **kwargs):  # noqa: E501
        """Hent ut en bestemt feature fra et dataset  # noqa: E501

        Henter ut en bestemt feature med mulighet for å få med refererte objekter for redigering.  # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.get_dataset_feature_with_http_info(x_client_product_version, dataset_id, lokal_id, references, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str lokal_id: Identifikasjon.Lokalid til objektet (required)
        :param str references: Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  `none` - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  `direct` - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  `all` - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra `direct` også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i `direct`. Både flaten og alle avgrensningskurvene låses.  (required)
        :param str locking_type: Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes. 
        :param int limit: Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med `rel=next` i [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med `rel=next`. 
        :param int crs_epsg: Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes. 
        :param bool normalized_for_visualization: Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden. 
        :return: object
                 If the method is called asynchronously,
                 returns the request thread.
        """

        all_params = ['x_client_product_version', 'dataset_id', 'lokal_id', 'references', 'locking_type', 'limit', 'crs_epsg', 'normalized_for_visualization']  # noqa: E501
        all_params.append('async_req')
        all_params.append('_return_http_data_only')
        all_params.append('_preload_content')
        all_params.append('_request_timeout')

        params = locals()
        for key, val in six.iteritems(params['kwargs']):
            if key not in all_params:
                raise TypeError(
                    "Got an unexpected keyword argument '%s'"
                    " to method get_dataset_feature" % key
                )
            params[key] = val
        del params['kwargs']
        # verify the required parameter 'x_client_product_version' is set
        if ('x_client_product_version' not in params or
                params['x_client_product_version'] is None):
            raise ValueError("Missing the required parameter `x_client_product_version` when calling `get_dataset_feature`")  # noqa: E501
        # verify the required parameter 'dataset_id' is set
        if ('dataset_id' not in params or
                params['dataset_id'] is None):
            raise ValueError("Missing the required parameter `dataset_id` when calling `get_dataset_feature`")  # noqa: E501
        # verify the required parameter 'lokal_id' is set
        if ('lokal_id' not in params or
                params['lokal_id'] is None):
            raise ValueError("Missing the required parameter `lokal_id` when calling `get_dataset_feature`")  # noqa: E501
        # verify the required parameter 'references' is set
        if ('references' not in params or
                params['references'] is None):
            raise ValueError("Missing the required parameter `references` when calling `get_dataset_feature`")  # noqa: E501

        collection_formats = {}

        path_params = {}
        if 'dataset_id' in params:
            path_params['datasetId'] = params['dataset_id']  # noqa: E501
        if 'lokal_id' in params:
            path_params['lokalId'] = params['lokal_id']  # noqa: E501

        query_params = []
        if 'locking_type' in params:
            query_params.append(('locking_type', params['locking_type']))  # noqa: E501
        if 'references' in params:
            query_params.append(('references', params['references']))  # noqa: E501
        if 'limit' in params:
            query_params.append(('limit', params['limit']))  # noqa: E501
        if 'crs_epsg' in params:
            query_params.append(('crs_EPSG', params['crs_epsg']))  # noqa: E501
        if 'normalized_for_visualization' in params:
            query_params.append(('normalized_for_visualization', params['normalized_for_visualization']))  # noqa: E501

        header_params = {}
        if 'x_client_product_version' in params:
            header_params['X-Client-Product-Version'] = params['x_client_product_version']  # noqa: E501

        form_params = []
        local_var_files = {}

        body_params = None
        # HTTP header `Accept`
        header_params['Accept'] = self.api_client.select_header_accept(
            ['application/vnd.kartverket.sosi+json; version=1.0', 'application/vnd.kartverket.sosi+gml; version=1.0'])  # noqa: E501

        # Authentication setting
        auth_settings = ['basicAuth']  # noqa: E501

        return self.api_client.call_api(
            '/datasets/{datasetId}/features/{lokalId}', 'GET',
            path_params,
            query_params,
            header_params,
            body=body_params,
            post_params=form_params,
            files=local_var_files,
            response_type='object',  # noqa: E501
            auth_settings=auth_settings,
            async_req=params.get('async_req'),
            _return_http_data_only=params.get('_return_http_data_only'),
            _preload_content=params.get('_preload_content', True),
            _request_timeout=params.get('_request_timeout'),
            collection_formats=collection_formats)

    def get_dataset_feature_attributes(self, dataset_id, lokal_id, **kwargs):  # noqa: E501
        """Hent ut egenskapene til en bestemt feature fra et dataset  # noqa: E501

        Henter ut alle egenskapene til en bestemt feature.  # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.get_dataset_feature_attributes(dataset_id, lokal_id, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param str dataset_id: UUID of the dataset to get (required)
        :param str lokal_id: Identifikasjon.Lokalid til objektet (required)
        :return: object
                 If the method is called asynchronously,
                 returns the request thread.
        """
        kwargs['_return_http_data_only'] = True
        if kwargs.get('async_req'):
            return self.get_dataset_feature_attributes_with_http_info(dataset_id, lokal_id, **kwargs)  # noqa: E501
        else:
            (data) = self.get_dataset_feature_attributes_with_http_info(dataset_id, lokal_id, **kwargs)  # noqa: E501
            return data

    def get_dataset_feature_attributes_with_http_info(self, dataset_id, lokal_id, **kwargs):  # noqa: E501
        """Hent ut egenskapene til en bestemt feature fra et dataset  # noqa: E501

        Henter ut alle egenskapene til en bestemt feature.  # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.get_dataset_feature_attributes_with_http_info(dataset_id, lokal_id, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param str dataset_id: UUID of the dataset to get (required)
        :param str lokal_id: Identifikasjon.Lokalid til objektet (required)
        :return: object
                 If the method is called asynchronously,
                 returns the request thread.
        """

        all_params = ['dataset_id', 'lokal_id']  # noqa: E501
        all_params.append('async_req')
        all_params.append('_return_http_data_only')
        all_params.append('_preload_content')
        all_params.append('_request_timeout')

        params = locals()
        for key, val in six.iteritems(params['kwargs']):
            if key not in all_params:
                raise TypeError(
                    "Got an unexpected keyword argument '%s'"
                    " to method get_dataset_feature_attributes" % key
                )
            params[key] = val
        del params['kwargs']
        # verify the required parameter 'dataset_id' is set
        if ('dataset_id' not in params or
                params['dataset_id'] is None):
            raise ValueError("Missing the required parameter `dataset_id` when calling `get_dataset_feature_attributes`")  # noqa: E501
        # verify the required parameter 'lokal_id' is set
        if ('lokal_id' not in params or
                params['lokal_id'] is None):
            raise ValueError("Missing the required parameter `lokal_id` when calling `get_dataset_feature_attributes`")  # noqa: E501

        collection_formats = {}

        path_params = {}
        if 'dataset_id' in params:
            path_params['datasetId'] = params['dataset_id']  # noqa: E501
        if 'lokal_id' in params:
            path_params['lokalId'] = params['lokal_id']  # noqa: E501

        query_params = []

        header_params = {}

        form_params = []
        local_var_files = {}

        body_params = None
        # HTTP header `Accept`
        header_params['Accept'] = self.api_client.select_header_accept(
            ['application/vnd.kartverket.ngis.attributes+json; version=1.0'])  # noqa: E501

        # Authentication setting
        auth_settings = ['basicAuth']  # noqa: E501

        return self.api_client.call_api(
            '/datasets/{datasetId}/features/{lokalId}/attributes', 'GET',
            path_params,
            query_params,
            header_params,
            body=body_params,
            post_params=form_params,
            files=local_var_files,
            response_type='object',  # noqa: E501
            auth_settings=auth_settings,
            async_req=params.get('async_req'),
            _return_http_data_only=params.get('_return_http_data_only'),
            _preload_content=params.get('_preload_content', True),
            _request_timeout=params.get('_request_timeout'),
            collection_formats=collection_formats)

    def get_dataset_features(self, x_client_product_version, dataset_id, references, **kwargs):  # noqa: E501
        """Hent ut features fra et dataset  # noqa: E501

        Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.  # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.get_dataset_features(x_client_product_version, dataset_id, references, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str references: Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  `none` - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  `direct` - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  `all` - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra `direct` også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i `direct`. Både flaten og alle avgrensningskurvene låses.  (required)
        :param str locking_type: Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes. 
        :param BoundingBox bbox: Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom `references` ikke er angitt blir det satt til `none`  
        :param int crs_epsg: Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes. 
        :param bool normalized_for_visualization: Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden. 
        :param int limit: Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med `rel=next` i [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med `rel=next`. 
        :param str cursor: Brukes til porsjonering av data
        :param str query: ### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun `eq` og `in` for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer. 
        :param datetime dataset_at: Angir hvilket tidspunkt (i henhold til ISO 8601, eks. `2020-01-30T12:13:14`) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks `crs_EPSG` osv. Kan foreløpig ikke kombineres med `query`, og ved bruk av `bbox` kan ikke `references` være annet enn `none` (hvis ikke angitt blir det satt til `none`). 
        :param str dataset_modified: ### Intervall for uthenting av historiske endringer  Må kombineres med `Accept`-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks `crs_EPSG` osv. Kan foreløpig ikke kombineres med `query`, og ved bruk av `bbox` kan ikke `references` være annet enn `none` (hvis ikke angitt blir det satt til `none`).  Formatet er <fra>/<til> hvor `..` er det samme som hhv. \"fra og med første endring\" eller \"til og med siste endring\".  Hvis man ikke angir `..` må man angi et tidspunkt i henhold til ISO 8601, eks. `2020-01-30T12:13:14`. Man kan også angi høyere oppløsning enn sekund og tidssone med `Z` (betyr UTC) eller offset f.eks `+02:00`. 
        :return: object
                 If the method is called asynchronously,
                 returns the request thread.
        """
        kwargs['_return_http_data_only'] = True
        if kwargs.get('async_req'):
            return self.get_dataset_features_with_http_info(x_client_product_version, dataset_id, references, **kwargs)  # noqa: E501
        else:
            (data) = self.get_dataset_features_with_http_info(x_client_product_version, dataset_id, references, **kwargs)  # noqa: E501
            return data

    def get_dataset_features_with_http_info(self, x_client_product_version, dataset_id, references, **kwargs):  # noqa: E501
        """Hent ut features fra et dataset  # noqa: E501

        Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.  # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.get_dataset_features_with_http_info(x_client_product_version, dataset_id, references, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str references: Angir hvilke refererte features som skal hentes ut i tillegg til feature det spørres direkte etter med lokalid.  Punkter har ingen referanser, og parameteret påvirker derfor ikke uthenting av et punkt.  `none` - Ingen refererte features hentes ut. Ingen ekstra features låses. - Flater får *ikke* med linje-features til avgrensningskurver.  `direct` - Features med direkte referanser hentes ut. Kun direkte referanser låses. - Kurver får med features til flater som bruker kurven i avgrensningen, samt de andre linje-features til avgrensningskurvene i flatene. Både kurven og alle flatene låses, men ikke de andre linje-features i avgrensningen i flatene. - Flater får med alle linje-features til avgrensningskurvene i flaten. Kun flaten låses.  `all` - Features med direkte og indirekte tilhørende referanser hentes ut. Alle referanser som hentes ut låses. - Kurver får i tillegg til referanser fra `direct` også med:   - flate-features som har avgrensningskurver som begynner eller slutter i valgt linjes endepunkter    - linje-features i flate-features i forrige pkt som begynner eller slutter i valgt linjes endepunkter  - Flater får samme referanser som i `direct`. Både flaten og alle avgrensningskurvene låses.  (required)
        :param str locking_type: Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes. 
        :param BoundingBox bbox: Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom `references` ikke er angitt blir det satt til `none`  
        :param int crs_epsg: Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes. 
        :param bool normalized_for_visualization: Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden. 
        :param int limit: Angir grense for antall objekter som skal returneres av gangen. Hvert dataset har en øvre grense som vil overstyre det klienten ber om, eller brukes dersom klienten ikke angir dette parameteret.  Dersom resultatet inneholder flere objekter enn grensen, vil responsen inneholde en lenke med `rel=next` i [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header. Denne lenken skal brukes direkte (det er ikke tillatt å tolke lenken ytterligere) for å hente ut neste del av resultatet, som vil være tilsvarende. De siste objektene fra operasjonen er hentet ut når resultatet ikke lenger inneholder en [`Link`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Link)-header med `rel=next`. 
        :param str cursor: Brukes til porsjonering av data
        :param str query: ### Filter på egenskaper og/eller objekttype for å begrense hvilke features som hentes ut.  Spørrespråket er basert på [RQL](https://github.com/persvr/rql).  Foreløpig støttes kun `eq` og `in` for bestemte objekttyper og egenskaper.  Eksemplene inneholder alle støttede spørringer. 
        :param datetime dataset_at: Angir hvilket tidspunkt (i henhold til ISO 8601, eks. `2020-01-30T12:13:14`) man ønsker å hente ut data på.  Kan kombineres med andre parametre, f.eks `crs_EPSG` osv. Kan foreløpig ikke kombineres med `query`, og ved bruk av `bbox` kan ikke `references` være annet enn `none` (hvis ikke angitt blir det satt til `none`). 
        :param str dataset_modified: ### Intervall for uthenting av historiske endringer  Må kombineres med `Accept`-header med samme dataformat som ved lagring av endringer.  Kan kombineres med andre parametre, f.eks `crs_EPSG` osv. Kan foreløpig ikke kombineres med `query`, og ved bruk av `bbox` kan ikke `references` være annet enn `none` (hvis ikke angitt blir det satt til `none`).  Formatet er <fra>/<til> hvor `..` er det samme som hhv. \"fra og med første endring\" eller \"til og med siste endring\".  Hvis man ikke angir `..` må man angi et tidspunkt i henhold til ISO 8601, eks. `2020-01-30T12:13:14`. Man kan også angi høyere oppløsning enn sekund og tidssone med `Z` (betyr UTC) eller offset f.eks `+02:00`. 
        :return: object
                 If the method is called asynchronously,
                 returns the request thread.
        """

        all_params = ['x_client_product_version', 'dataset_id', 'references', 'locking_type', 'bbox', 'crs_epsg', 'normalized_for_visualization', 'limit', 'cursor', 'query', 'dataset_at', 'dataset_modified']  # noqa: E501
        all_params.append('async_req')
        all_params.append('_return_http_data_only')
        all_params.append('_preload_content')
        all_params.append('_request_timeout')

        params = locals()
        for key, val in six.iteritems(params['kwargs']):
            if key not in all_params:
                raise TypeError(
                    "Got an unexpected keyword argument '%s'"
                    " to method get_dataset_features" % key
                )
            params[key] = val
        del params['kwargs']
        # verify the required parameter 'x_client_product_version' is set
        if ('x_client_product_version' not in params or
                params['x_client_product_version'] is None):
            raise ValueError("Missing the required parameter `x_client_product_version` when calling `get_dataset_features`")  # noqa: E501
        # verify the required parameter 'dataset_id' is set
        if ('dataset_id' not in params or
                params['dataset_id'] is None):
            raise ValueError("Missing the required parameter `dataset_id` when calling `get_dataset_features`")  # noqa: E501
        # verify the required parameter 'references' is set
        if ('references' not in params or
                params['references'] is None):
            raise ValueError("Missing the required parameter `references` when calling `get_dataset_features`")  # noqa: E501

        collection_formats = {}

        path_params = {}
        if 'dataset_id' in params:
            path_params['datasetId'] = params['dataset_id']  # noqa: E501

        query_params = []
        if 'locking_type' in params:
            query_params.append(('locking_type', params['locking_type']))  # noqa: E501
        if 'bbox' in params:
            query_params.append(('bbox', params['bbox']))  # noqa: E501
        if 'crs_epsg' in params:
            query_params.append(('crs_EPSG', params['crs_epsg']))  # noqa: E501
        if 'normalized_for_visualization' in params:
            query_params.append(('normalized_for_visualization', params['normalized_for_visualization']))  # noqa: E501
        if 'references' in params:
            query_params.append(('references', params['references']))  # noqa: E501
        if 'limit' in params:
            query_params.append(('limit', params['limit']))  # noqa: E501
        if 'cursor' in params:
            query_params.append(('cursor', params['cursor']))  # noqa: E501
        if 'query' in params:
            query_params.append(('query', params['query']))  # noqa: E501
        if 'dataset_at' in params:
            query_params.append(('dataset_at', params['dataset_at']))  # noqa: E501
        if 'dataset_modified' in params:
            query_params.append(('dataset_modified', params['dataset_modified']))  # noqa: E501

        header_params = {}
        if 'x_client_product_version' in params:
            header_params['X-Client-Product-Version'] = params['x_client_product_version']  # noqa: E501

        form_params = []
        local_var_files = {}

        body_params = None
        # HTTP header `Accept`
        header_params['Accept'] = self.api_client.select_header_accept(
            ['application/vnd.kartverket.sosi+json; version=1.0', 'application/vnd.kartverket.sosi+json; version=2.0', 'application/vnd.kartverket.sosi+gml; version=1.0'])  # noqa: E501

        # Authentication setting
        auth_settings = ['basicAuth']  # noqa: E501

        return self.api_client.call_api(
            '/datasets/{datasetId}/features', 'GET',
            path_params,
            query_params,
            header_params,
            body=body_params,
            post_params=form_params,
            files=local_var_files,
            response_type='object',  # noqa: E501
            auth_settings=auth_settings,
            async_req=params.get('async_req'),
            _return_http_data_only=params.get('_return_http_data_only'),
            _preload_content=params.get('_preload_content', True),
            _request_timeout=params.get('_request_timeout'),
            collection_formats=collection_formats)

    def update_dataset_feature_attributes(self, body, x_client_product_version, dataset_id, lokal_id, **kwargs):  # noqa: E501
        """Endre egenskapene til en bestemt feature i et dataset  # noqa: E501

        Oppdaterer alle egenskapene til en bestemt feature i et dataset.  # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.update_dataset_feature_attributes(body, x_client_product_version, dataset_id, lokal_id, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param object body: (required)
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str lokal_id: Identifikasjon.Lokalid til objektet (required)
        :return: object
                 If the method is called asynchronously,
                 returns the request thread.
        """
        kwargs['_return_http_data_only'] = True
        if kwargs.get('async_req'):
            return self.update_dataset_feature_attributes_with_http_info(body, x_client_product_version, dataset_id, lokal_id, **kwargs)  # noqa: E501
        else:
            (data) = self.update_dataset_feature_attributes_with_http_info(body, x_client_product_version, dataset_id, lokal_id, **kwargs)  # noqa: E501
            return data

    def update_dataset_feature_attributes_with_http_info(self, body, x_client_product_version, dataset_id, lokal_id, **kwargs):  # noqa: E501
        """Endre egenskapene til en bestemt feature i et dataset  # noqa: E501

        Oppdaterer alle egenskapene til en bestemt feature i et dataset.  # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.update_dataset_feature_attributes_with_http_info(body, x_client_product_version, dataset_id, lokal_id, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param object body: (required)
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str lokal_id: Identifikasjon.Lokalid til objektet (required)
        :return: object
                 If the method is called asynchronously,
                 returns the request thread.
        """

        all_params = ['body', 'x_client_product_version', 'dataset_id', 'lokal_id']  # noqa: E501
        all_params.append('async_req')
        all_params.append('_return_http_data_only')
        all_params.append('_preload_content')
        all_params.append('_request_timeout')

        params = locals()
        for key, val in six.iteritems(params['kwargs']):
            if key not in all_params:
                raise TypeError(
                    "Got an unexpected keyword argument '%s'"
                    " to method update_dataset_feature_attributes" % key
                )
            params[key] = val
        del params['kwargs']
        # verify the required parameter 'body' is set
        if ('body' not in params or
                params['body'] is None):
            raise ValueError("Missing the required parameter `body` when calling `update_dataset_feature_attributes`")  # noqa: E501
        # verify the required parameter 'x_client_product_version' is set
        if ('x_client_product_version' not in params or
                params['x_client_product_version'] is None):
            raise ValueError("Missing the required parameter `x_client_product_version` when calling `update_dataset_feature_attributes`")  # noqa: E501
        # verify the required parameter 'dataset_id' is set
        if ('dataset_id' not in params or
                params['dataset_id'] is None):
            raise ValueError("Missing the required parameter `dataset_id` when calling `update_dataset_feature_attributes`")  # noqa: E501
        # verify the required parameter 'lokal_id' is set
        if ('lokal_id' not in params or
                params['lokal_id'] is None):
            raise ValueError("Missing the required parameter `lokal_id` when calling `update_dataset_feature_attributes`")  # noqa: E501

        collection_formats = {}

        path_params = {}
        if 'dataset_id' in params:
            path_params['datasetId'] = params['dataset_id']  # noqa: E501
        if 'lokal_id' in params:
            path_params['lokalId'] = params['lokal_id']  # noqa: E501

        query_params = []

        header_params = {}
        if 'x_client_product_version' in params:
            header_params['X-Client-Product-Version'] = params['x_client_product_version']  # noqa: E501

        form_params = []
        local_var_files = {}

        body_params = None
        if 'body' in params:
            body_params = params['body']
        # HTTP header `Accept`
        header_params['Accept'] = self.api_client.select_header_accept(
            ['application/vnd.kartverket.ngis.attributes+json; version=1.0'])  # noqa: E501

        # HTTP header `Content-Type`
        header_params['Content-Type'] = self.api_client.select_header_content_type(  # noqa: E501
            ['application/vnd.kartverket.ngis.attributes+json; version=1.0'])  # noqa: E501

        # Authentication setting
        auth_settings = ['basicAuth']  # noqa: E501

        return self.api_client.call_api(
            '/datasets/{datasetId}/features/{lokalId}/attributes', 'PUT',
            path_params,
            query_params,
            header_params,
            body=body_params,
            post_params=form_params,
            files=local_var_files,
            response_type='object',  # noqa: E501
            auth_settings=auth_settings,
            async_req=params.get('async_req'),
            _return_http_data_only=params.get('_return_http_data_only'),
            _preload_content=params.get('_preload_content', True),
            _request_timeout=params.get('_request_timeout'),
            collection_formats=collection_formats)

    def update_dataset_features(self, body, x_client_product_version, dataset_id, **kwargs):  # noqa: E501
        """Endre features i et dataset  # noqa: E501

        Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.  # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.update_dataset_features(body, x_client_product_version, dataset_id, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param object body: Optional description in *Markdown* (required)
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str locking_type: Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes. 
        :param int crs_epsg: Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes. 
        :param bool normalized_for_visualization: Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden. 
        :param bool _async: Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder. 
        :param str copy_transaction_token: Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet 
        :return: InlineResponse200
                 If the method is called asynchronously,
                 returns the request thread.
        """
        kwargs['_return_http_data_only'] = True
        if kwargs.get('async_req'):
            return self.update_dataset_features_with_http_info(body, x_client_product_version, dataset_id, **kwargs)  # noqa: E501
        else:
            (data) = self.update_dataset_features_with_http_info(body, x_client_product_version, dataset_id, **kwargs)  # noqa: E501
            return data

    def update_dataset_features_with_http_info(self, body, x_client_product_version, dataset_id, **kwargs):  # noqa: E501
        """Endre features i et dataset  # noqa: E501

        Henter en liste over alle dataset som brukeren har lese- eller skrivetilgang til.  # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.update_dataset_features_with_http_info(body, x_client_product_version, dataset_id, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param object body: Optional description in *Markdown* (required)
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str locking_type: Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes. 
        :param int crs_epsg: Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes. 
        :param bool normalized_for_visualization: Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden. 
        :param bool _async: Angir at dataene skal behandles asynkront, slik at klienten slipper å vente til skriving er ferdig. Påkrevet ved større datamengder. 
        :param str copy_transaction_token: Angir at dataene som skrives er kopidata, og at hele identifikasjons-egenskapen skal beholdes.  Verdien angir versjonsnummer for oppdatering fra originalarkivet 
        :return: InlineResponse200
                 If the method is called asynchronously,
                 returns the request thread.
        """

        all_params = ['body', 'x_client_product_version', 'dataset_id', 'locking_type', 'crs_epsg', 'normalized_for_visualization', '_async', 'copy_transaction_token']  # noqa: E501
        all_params.append('async_req')
        all_params.append('_return_http_data_only')
        all_params.append('_preload_content')
        all_params.append('_request_timeout')

        params = locals()
        for key, val in six.iteritems(params['kwargs']):
            if key not in all_params:
                raise TypeError(
                    "Got an unexpected keyword argument '%s'"
                    " to method update_dataset_features" % key
                )
            params[key] = val
        del params['kwargs']
        # verify the required parameter 'body' is set
        if ('body' not in params or
                params['body'] is None):
            raise ValueError("Missing the required parameter `body` when calling `update_dataset_features`")  # noqa: E501
        # verify the required parameter 'x_client_product_version' is set
        if ('x_client_product_version' not in params or
                params['x_client_product_version'] is None):
            raise ValueError("Missing the required parameter `x_client_product_version` when calling `update_dataset_features`")  # noqa: E501
        # verify the required parameter 'dataset_id' is set
        if ('dataset_id' not in params or
                params['dataset_id'] is None):
            raise ValueError("Missing the required parameter `dataset_id` when calling `update_dataset_features`")  # noqa: E501

        collection_formats = {}

        path_params = {}
        if 'dataset_id' in params:
            path_params['datasetId'] = params['dataset_id']  # noqa: E501

        query_params = []
        if 'locking_type' in params:
            query_params.append(('locking_type', params['locking_type']))  # noqa: E501
        if 'crs_epsg' in params:
            query_params.append(('crs_EPSG', params['crs_epsg']))  # noqa: E501
        if 'normalized_for_visualization' in params:
            query_params.append(('normalized_for_visualization', params['normalized_for_visualization']))  # noqa: E501
        if '_async' in params:
            query_params.append(('async', params['_async']))  # noqa: E501
        if 'copy_transaction_token' in params:
            query_params.append(('copy_transaction_token', params['copy_transaction_token']))  # noqa: E501

        header_params = {}
        if 'x_client_product_version' in params:
            header_params['X-Client-Product-Version'] = params['x_client_product_version']  # noqa: E501

        form_params = []
        local_var_files = {}

        body_params = None
        if 'body' in params:
            body_params = params['body']
        # HTTP header `Accept`
        header_params['Accept'] = self.api_client.select_header_accept(
            ['application/vnd.kartverket.ngis.edit_features_summary+json'])  # noqa: E501

        # HTTP header `Content-Type`
        header_params['Content-Type'] = self.api_client.select_header_content_type(  # noqa: E501
            ['application/vnd.kartverket.sosi+wfs-t; version=1.0', 'application/vnd.kartverket.geosynkronisering+zip; version=1.0', 'application/vnd.kartverket.sosi+json; version=1.0', 'application/vnd.kartverket.sosi+json; version=2.0'])  # noqa: E501

        # Authentication setting
        auth_settings = ['basicAuth']  # noqa: E501

        return self.api_client.call_api(
            '/datasets/{datasetId}/features', 'POST',
            path_params,
            query_params,
            header_params,
            body=body_params,
            post_params=form_params,
            files=local_var_files,
            response_type='InlineResponse200',  # noqa: E501
            auth_settings=auth_settings,
            async_req=params.get('async_req'),
            _return_http_data_only=params.get('_return_http_data_only'),
            _preload_content=params.get('_preload_content', True),
            _request_timeout=params.get('_request_timeout'),
            collection_formats=collection_formats)
