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


class LocksApi(object):
    """NOTE: This class is auto generated by the swagger code generator program.

    Do not edit the class manually.
    Ref: https://github.com/swagger-api/swagger-codegen
    """

    def __init__(self, api_client=None):
        if api_client is None:
            api_client = ApiClient()
        self.api_client = api_client

    def delete_dataset_locks(self, x_client_product_version, dataset_id, **kwargs):  # noqa: E501
        """Fjerne alle låser brukeren har i et bestemt dataset  # noqa: E501

        Fjerne alle låser brukeren har i et bestemt dataset   # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.delete_dataset_locks(x_client_product_version, dataset_id, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str locking_type: Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes. 
        :return: None
                 If the method is called asynchronously,
                 returns the request thread.
        """
        kwargs['_return_http_data_only'] = True
        if kwargs.get('async_req'):
            return self.delete_dataset_locks_with_http_info(x_client_product_version, dataset_id, **kwargs)  # noqa: E501
        else:
            (data) = self.delete_dataset_locks_with_http_info(x_client_product_version, dataset_id, **kwargs)  # noqa: E501
            return data

    def delete_dataset_locks_with_http_info(self, x_client_product_version, dataset_id, **kwargs):  # noqa: E501
        """Fjerne alle låser brukeren har i et bestemt dataset  # noqa: E501

        Fjerne alle låser brukeren har i et bestemt dataset   # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.delete_dataset_locks_with_http_info(x_client_product_version, dataset_id, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str locking_type: Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes. 
        :return: None
                 If the method is called asynchronously,
                 returns the request thread.
        """

        all_params = ['x_client_product_version', 'dataset_id', 'locking_type']  # noqa: E501
        all_params.append('async_req')
        all_params.append('_return_http_data_only')
        all_params.append('_preload_content')
        all_params.append('_request_timeout')

        params = locals()
        for key, val in six.iteritems(params['kwargs']):
            if key not in all_params:
                raise TypeError(
                    "Got an unexpected keyword argument '%s'"
                    " to method delete_dataset_locks" % key
                )
            params[key] = val
        del params['kwargs']
        # verify the required parameter 'x_client_product_version' is set
        if ('x_client_product_version' not in params or
                params['x_client_product_version'] is None):
            raise ValueError("Missing the required parameter `x_client_product_version` when calling `delete_dataset_locks`")  # noqa: E501
        # verify the required parameter 'dataset_id' is set
        if ('dataset_id' not in params or
                params['dataset_id'] is None):
            raise ValueError("Missing the required parameter `dataset_id` when calling `delete_dataset_locks`")  # noqa: E501

        collection_formats = {}

        path_params = {}
        if 'dataset_id' in params:
            path_params['datasetId'] = params['dataset_id']  # noqa: E501

        query_params = []
        if 'locking_type' in params:
            query_params.append(('locking_type', params['locking_type']))  # noqa: E501

        header_params = {}
        if 'x_client_product_version' in params:
            header_params['X-Client-Product-Version'] = params['x_client_product_version']  # noqa: E501

        form_params = []
        local_var_files = {}

        body_params = None
        # Authentication setting
        auth_settings = ['basicAuth']  # noqa: E501

        return self.api_client.call_api(
            '/datasets/{datasetId}/locks', 'DELETE',
            path_params,
            query_params,
            header_params,
            body=body_params,
            post_params=form_params,
            files=local_var_files,
            response_type=None,  # noqa: E501
            auth_settings=auth_settings,
            async_req=params.get('async_req'),
            _return_http_data_only=params.get('_return_http_data_only'),
            _preload_content=params.get('_preload_content', True),
            _request_timeout=params.get('_request_timeout'),
            collection_formats=collection_formats)

    def get_dataset_locks(self, x_client_product_version, dataset_id, **kwargs):  # noqa: E501
        """Hent informasjon om brukerens låste features i et bestemt dataset. Bruk `application/vnd.kartverket.ngis.locks+json` for kun å se egne låser, og `application/vnd.kartverket.ngis.all_locks+json` for også å se andres låser.  # noqa: E501

        Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset.   # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.get_dataset_locks(x_client_product_version, dataset_id, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str locking_type: Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes. 
        :param BoundingBox bbox: Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom `references` ikke er angitt blir det satt til `none`  
        :param int crs_epsg: Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes. 
        :param bool normalized_for_visualization: Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden. 
        :return: Locks
                 If the method is called asynchronously,
                 returns the request thread.
        """
        kwargs['_return_http_data_only'] = True
        if kwargs.get('async_req'):
            return self.get_dataset_locks_with_http_info(x_client_product_version, dataset_id, **kwargs)  # noqa: E501
        else:
            (data) = self.get_dataset_locks_with_http_info(x_client_product_version, dataset_id, **kwargs)  # noqa: E501
            return data

    def get_dataset_locks_with_http_info(self, x_client_product_version, dataset_id, **kwargs):  # noqa: E501
        """Hent informasjon om brukerens låste features i et bestemt dataset. Bruk `application/vnd.kartverket.ngis.locks+json` for kun å se egne låser, og `application/vnd.kartverket.ngis.all_locks+json` for også å se andres låser.  # noqa: E501

        Henter bl.a. informasjon om hvilke objekter brukeren har låst i et bestemt dataset.   # noqa: E501
        This method makes a synchronous HTTP request by default. To make an
        asynchronous HTTP request, please pass async_req=True
        >>> thread = api.get_dataset_locks_with_http_info(x_client_product_version, dataset_id, async_req=True)
        >>> result = thread.get()

        :param async_req bool
        :param str x_client_product_version: Brukes for å kunne identifisere klienten som er brukt (required)
        :param str dataset_id: UUID of the dataset to get (required)
        :param str locking_type: Angir låsetype som skal brukes (foreløpig er kun `user_lock` støttet). Krever at brukeren har skrivetilgang mot dataset'et.  *user_lock*  Hver bruker har én lås per dataset. Hver gang data hentes ut med `user_lock` legges objektene til denne låsen.  Alle objekter i låsen låses opp neste gang brukeren skriver data til dataset'et.  Låsen vil fjernes neste gang brukeren skriver data til dataset'et med `user_lock`, eller dersom låsen slettes. 
        :param BoundingBox bbox: Henter ut objekter i et bestemt område (angitt med et rektangel)  Dette kan være aktuelt for å få ut alle objekter som ikke er direkte knyttet til et objekt, som f.eks mønelinje og takkant til en bygning.  Merk: Dersom `references` ikke er angitt blir det satt til `none`  
        :param int crs_epsg: Angir EPSG-kode for koordinatsystemet til koordinatene som sendes inn i spørringen (f.eks i bbox), som sendes inn som data, og som sendes tilbake. Påkrevd dersom bbox-parameteret brukes. 
        :param bool normalized_for_visualization: Angir at rekkefølgen på x- og y-aksen skal være snudd mot det som er spesifisert i EPSG-koden. 
        :return: Locks
                 If the method is called asynchronously,
                 returns the request thread.
        """

        all_params = ['x_client_product_version', 'dataset_id', 'locking_type', 'bbox', 'crs_epsg', 'normalized_for_visualization']  # noqa: E501
        all_params.append('async_req')
        all_params.append('_return_http_data_only')
        all_params.append('_preload_content')
        all_params.append('_request_timeout')

        params = locals()
        for key, val in six.iteritems(params['kwargs']):
            if key not in all_params:
                raise TypeError(
                    "Got an unexpected keyword argument '%s'"
                    " to method get_dataset_locks" % key
                )
            params[key] = val
        del params['kwargs']
        # verify the required parameter 'x_client_product_version' is set
        if ('x_client_product_version' not in params or
                params['x_client_product_version'] is None):
            raise ValueError("Missing the required parameter `x_client_product_version` when calling `get_dataset_locks`")  # noqa: E501
        # verify the required parameter 'dataset_id' is set
        if ('dataset_id' not in params or
                params['dataset_id'] is None):
            raise ValueError("Missing the required parameter `dataset_id` when calling `get_dataset_locks`")  # noqa: E501

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

        header_params = {}
        if 'x_client_product_version' in params:
            header_params['X-Client-Product-Version'] = params['x_client_product_version']  # noqa: E501

        form_params = []
        local_var_files = {}

        body_params = None
        # HTTP header `Accept`
        header_params['Accept'] = self.api_client.select_header_accept(
            ['application/vnd.kartverket.ngis.locks+json', 'application/vnd.kartverket.ngis.all_locks+json'])  # noqa: E501

        # Authentication setting
        auth_settings = ['basicAuth']  # noqa: E501

        return self.api_client.call_api(
            '/datasets/{datasetId}/locks', 'GET',
            path_params,
            query_params,
            header_params,
            body=body_params,
            post_params=form_params,
            files=local_var_files,
            response_type='Locks',  # noqa: E501
            auth_settings=auth_settings,
            async_req=params.get('async_req'),
            _return_http_data_only=params.get('_return_http_data_only'),
            _preload_content=params.get('_preload_content', True),
            _request_timeout=params.get('_request_timeout'),
            collection_formats=collection_formats)
