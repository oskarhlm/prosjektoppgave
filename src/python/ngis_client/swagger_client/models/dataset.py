# coding: utf-8

"""
    Oppdateringsgrensesnitt for SFKB

    # NGIS-OpenAPI  Grov oversikt over funksjonalitet:   - Hente liste over tilgjengelige datasett    - Hente metadata for et bestemt datasett   - Hente data fra et bestemt datasett     - Med lesetilgang eller skrivetilgang (medfører låsing)       - områdebegrensning       - egenskapsspørring (begrenset i første versjon til bygningsnummer eller lokalid)   - Lagre data til et bestemt datasett     - Operasjoner som håndteres: nytt objekt, endre objekt og slett objekt  ## Generelle prinsipper for systemet  ### Versjonering og bakoverkompatibilitet  #### Versjonsnummer i URL  Vi har et versjonsnummer `v1` i URL for å gjøre det mulig å gjøre store endringer i APIet hvis det blir nødvendig, men i utgangspunktet ønsker vi å unngå å endre dette versjonsnummeret.  #### Versjonsnummer i media types (\"media type versioning\", content negotiation)  APIet baserer seg på standard [HTTP content negotiation](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept) ved utveksling av data med headerne `Accept` og `Content-Type`. Dette gjør det veldig enkelt å introdusere nye dataformater i APIet uten endringer for eksisterende klienter. I tillegg til dette inneholder også alle dataformater et versjonsparameter, eks. `version=1.0`, der klienten kan styre hvilket  eller hvilke dataformater klienten kan håndtere. Dataformater angitt uten versjonsparameter vil tolkes som å be om siste versjon.  `Accept: application/vnd.kartverket.sosi+json; version=1.0` Klienten ønsker svar med versjon 1 av dataformatet      `Accept: application/vnd.kartverket.sosi+json; version=2.0` Klienten ønsker svar med versjon 2 av dataformatet  `Accept: application/vnd.kartverket.sosi+json` Klienten ønsker svar med siste versjon av dataformatet  `Accept: application/vnd.kartverket.sosi+json; version=1.0, application/vnd.kartverket.sosi+json; version=2.0` Klienten håndterer både versjon 1.0 og 2.0 av dataformatet, og gir derfor tjenesten mulighet til å velge hvilken som skal brukes  `Accept: */*` Klienten håndterer alle dataformater, og gir derfor tjenesten mulighet til å velge hvilken som skal brukes.  Der man kan velge mellom flere helt ulike dataformater som f.eks GML og JSON, må man faktisk håndtere begge.  ### Delt geometri  Flater består av avgrensningslinjer som ligger lagret som egne objekter. På den måten kan en linje avgrense ingen, én eller flere flater. Det er likevel slik at flater hentes ut og lagres med egen geometri for å gjøre det enklere å tegne opp datene, men ved endring av (delte) linjer og flater må det tas hensyn til delt geometri. Forsøk på endring av linje eller flate uten tilsvarende endring av evt. delt geometri vil bli avvist av systemet.  ### Låsing  Dette er nærmere beskrevet i de aktuelle kallene.  Foreløpig er det kun `user_lock` som er støttet. Det betyr at data må hentes ut med `user_lock` før de kan sendes inn med endringer.  ### Porsjonering  All uthenting av feature-objekter vil kunne bli porsjonert av serveren, se `limit`-parameteret.   ### Koordinatsystemer og transformasjon  Dersom annet koordinatsystem enn det som ligger i dataset skal brukes (se `GET /datasets/{datasetId}`) må koordinatsystem angis med `crs_EPSG`-parameteret. Dette styrer data som sendes inn, data som hentes ut og koordinatsystemet i `bbox`-parameteret i kallet. For å bytte rekkefølge på aksene brukes `crs_normalized_for_visualization`-parameteret.  ### Historikk og historiske endringer  Det er mulig å hente ut data for et gitt tidspunkt (for hele datasettet eller begrenset til et område, et objekt etc.). Se etter parameteret `dataset_at`.  Det er også mulig å hente ut historikken som endringer, f.eks som endringer fra et tidligere uthentet område, objekt eller helt dataset. Se etter parameteret `dataset_modified`.  ### FKB5 og QMS13  I forbindelse med FKB5 har det blitt gjort endringer i GeoJSON-formatet som benyttes i NGIS-OpenAPI. Endringene gjelder ved bruk av Versjon 2 av formatet,  som er påkrevd versjon for FKB 5.  Endringer i GeoJSON-formatet, Versjon 2:   - Noden `geometry_properties` er flyttet under `properties`   - Formatet støtter assosiasjoner mellom objekter og geometri-assosiasjoner ved flater med delt geometri   - Formatet støtter heleid geometri  #### Assosiasjoner mellom objekter Et objekt kan ha en assosiasjon til ett eller flere andre objekter. Assosiasjonene av samme type ligger i en array med `lokalId` og `featuretype` til det assosierte objektet.  Flater kan være modellert med krav om delt geometri for flateavgrensningen. Dette angis via geometri-assosiasjoner fra flate-objektet til linjene-objektene som avgrenser flata.  I tillegg til assosiasjons-informasjonen beskrevet over, har geometri-assosiasjonene følgende egenskaper for hvert assosiert objekt:      `reverse` er en bool som forteller om linjas retning skal snus eller ikke for å danne en sammenhengende flateavgrensing med de andre avgrensningslinjene.  `idx` er ett array med tre indekser som gir informasjon om avgrensningslinja:    1. angir hvilken flate linja tilhører (aktuell ved framtidig bruk av MultiSurface), skal for enkeltflater være 0    2. angir om linja tilhører ytre eller en indre avgrensning (hull) og eventuelt hvilken indre avgrensning linja tilhører       - Ytre avgrensning: 0       - Indre avgrensning/hull: 1..n    3. angir hvilken rekkefølge linja har i avgrensningen av flata som dannes av alle avgrensninslinjene (starter på 0)   # noqa: E501

    OpenAPI spec version: 1.0.0
    
    Generated by: https://github.com/swagger-api/swagger-codegen.git
"""

import pprint
import re  # noqa: F401

import six

class Dataset(object):
    """NOTE: This class is auto generated by the swagger code generator program.

    Do not edit the class manually.
    """
    """
    Attributes:
      swagger_types (dict): The key is attribute name
                            and the value is attribute type.
      attribute_map (dict): The key is attribute name
                            and the value is json key in definition.
    """
    swagger_types = {
        'id': 'str',
        'name': 'str',
        'description': 'str',
        'resolution': 'float',
        'crs_epsg': 'int',
        'dataset_last_modified': 'str',
        'schema_url': 'str',
        'access': 'str',
        'bbox': 'BoundingBox',
        'copy_transaction_token': 'str'
    }

    attribute_map = {
        'id': 'id',
        'name': 'name',
        'description': 'description',
        'resolution': 'resolution',
        'crs_epsg': 'crs_EPSG',
        'dataset_last_modified': 'dataset_last_modified',
        'schema_url': 'schema_url',
        'access': 'access',
        'bbox': 'bbox',
        'copy_transaction_token': 'copy_transaction_token'
    }

    def __init__(self, id=None, name=None, description=None, resolution=None, crs_epsg=None, dataset_last_modified=None, schema_url=None, access=None, bbox=None, copy_transaction_token=None):  # noqa: E501
        """Dataset - a model defined in Swagger"""  # noqa: E501
        self._id = None
        self._name = None
        self._description = None
        self._resolution = None
        self._crs_epsg = None
        self._dataset_last_modified = None
        self._schema_url = None
        self._access = None
        self._bbox = None
        self._copy_transaction_token = None
        self.discriminator = None
        if id is not None:
            self.id = id
        if name is not None:
            self.name = name
        if description is not None:
            self.description = description
        if resolution is not None:
            self.resolution = resolution
        if crs_epsg is not None:
            self.crs_epsg = crs_epsg
        if dataset_last_modified is not None:
            self.dataset_last_modified = dataset_last_modified
        if schema_url is not None:
            self.schema_url = schema_url
        if access is not None:
            self.access = access
        if bbox is not None:
            self.bbox = bbox
        if copy_transaction_token is not None:
            self.copy_transaction_token = copy_transaction_token

    @property
    def id(self):
        """Gets the id of this Dataset.  # noqa: E501


        :return: The id of this Dataset.  # noqa: E501
        :rtype: str
        """
        return self._id

    @id.setter
    def id(self, id):
        """Sets the id of this Dataset.


        :param id: The id of this Dataset.  # noqa: E501
        :type: str
        """

        self._id = id

    @property
    def name(self):
        """Gets the name of this Dataset.  # noqa: E501


        :return: The name of this Dataset.  # noqa: E501
        :rtype: str
        """
        return self._name

    @name.setter
    def name(self, name):
        """Sets the name of this Dataset.


        :param name: The name of this Dataset.  # noqa: E501
        :type: str
        """

        self._name = name

    @property
    def description(self):
        """Gets the description of this Dataset.  # noqa: E501


        :return: The description of this Dataset.  # noqa: E501
        :rtype: str
        """
        return self._description

    @description.setter
    def description(self, description):
        """Sets the description of this Dataset.


        :param description: The description of this Dataset.  # noqa: E501
        :type: str
        """

        self._description = description

    @property
    def resolution(self):
        """Gets the resolution of this Dataset.  # noqa: E501


        :return: The resolution of this Dataset.  # noqa: E501
        :rtype: float
        """
        return self._resolution

    @resolution.setter
    def resolution(self, resolution):
        """Sets the resolution of this Dataset.


        :param resolution: The resolution of this Dataset.  # noqa: E501
        :type: float
        """

        self._resolution = resolution

    @property
    def crs_epsg(self):
        """Gets the crs_epsg of this Dataset.  # noqa: E501


        :return: The crs_epsg of this Dataset.  # noqa: E501
        :rtype: int
        """
        return self._crs_epsg

    @crs_epsg.setter
    def crs_epsg(self, crs_epsg):
        """Sets the crs_epsg of this Dataset.


        :param crs_epsg: The crs_epsg of this Dataset.  # noqa: E501
        :type: int
        """

        self._crs_epsg = crs_epsg

    @property
    def dataset_last_modified(self):
        """Gets the dataset_last_modified of this Dataset.  # noqa: E501


        :return: The dataset_last_modified of this Dataset.  # noqa: E501
        :rtype: str
        """
        return self._dataset_last_modified

    @dataset_last_modified.setter
    def dataset_last_modified(self, dataset_last_modified):
        """Sets the dataset_last_modified of this Dataset.


        :param dataset_last_modified: The dataset_last_modified of this Dataset.  # noqa: E501
        :type: str
        """

        self._dataset_last_modified = dataset_last_modified

    @property
    def schema_url(self):
        """Gets the schema_url of this Dataset.  # noqa: E501


        :return: The schema_url of this Dataset.  # noqa: E501
        :rtype: str
        """
        return self._schema_url

    @schema_url.setter
    def schema_url(self, schema_url):
        """Sets the schema_url of this Dataset.


        :param schema_url: The schema_url of this Dataset.  # noqa: E501
        :type: str
        """

        self._schema_url = schema_url

    @property
    def access(self):
        """Gets the access of this Dataset.  # noqa: E501


        :return: The access of this Dataset.  # noqa: E501
        :rtype: str
        """
        return self._access

    @access.setter
    def access(self, access):
        """Sets the access of this Dataset.


        :param access: The access of this Dataset.  # noqa: E501
        :type: str
        """
        allowed_values = ["read_only", "read_write"]  # noqa: E501
        if access not in allowed_values:
            raise ValueError(
                "Invalid value for `access` ({0}), must be one of {1}"  # noqa: E501
                .format(access, allowed_values)
            )

        self._access = access

    @property
    def bbox(self):
        """Gets the bbox of this Dataset.  # noqa: E501


        :return: The bbox of this Dataset.  # noqa: E501
        :rtype: BoundingBox
        """
        return self._bbox

    @bbox.setter
    def bbox(self, bbox):
        """Sets the bbox of this Dataset.


        :param bbox: The bbox of this Dataset.  # noqa: E501
        :type: BoundingBox
        """

        self._bbox = bbox

    @property
    def copy_transaction_token(self):
        """Gets the copy_transaction_token of this Dataset.  # noqa: E501


        :return: The copy_transaction_token of this Dataset.  # noqa: E501
        :rtype: str
        """
        return self._copy_transaction_token

    @copy_transaction_token.setter
    def copy_transaction_token(self, copy_transaction_token):
        """Sets the copy_transaction_token of this Dataset.


        :param copy_transaction_token: The copy_transaction_token of this Dataset.  # noqa: E501
        :type: str
        """

        self._copy_transaction_token = copy_transaction_token

    def to_dict(self):
        """Returns the model properties as a dict"""
        result = {}

        for attr, _ in six.iteritems(self.swagger_types):
            value = getattr(self, attr)
            if isinstance(value, list):
                result[attr] = list(map(
                    lambda x: x.to_dict() if hasattr(x, "to_dict") else x,
                    value
                ))
            elif hasattr(value, "to_dict"):
                result[attr] = value.to_dict()
            elif isinstance(value, dict):
                result[attr] = dict(map(
                    lambda item: (item[0], item[1].to_dict())
                    if hasattr(item[1], "to_dict") else item,
                    value.items()
                ))
            else:
                result[attr] = value
        if issubclass(Dataset, dict):
            for key, value in self.items():
                result[key] = value

        return result

    def to_str(self):
        """Returns the string representation of the model"""
        return pprint.pformat(self.to_dict())

    def __repr__(self):
        """For `print` and `pprint`"""
        return self.to_str()

    def __eq__(self, other):
        """Returns true if both objects are equal"""
        if not isinstance(other, Dataset):
            return False

        return self.__dict__ == other.__dict__

    def __ne__(self, other):
        """Returns true if both objects are not equal"""
        return not self == other
