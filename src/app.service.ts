import { Injectable, HttpService } from '@nestjs/common';

import { Twilio } from 'twilio';
import * as jsonxml from 'jsontoxml';
import * as fs from 'fs';
import * as uuid from 'uuid';

@Injectable()
export class AppService {

  constructor(private readonly httpClient: HttpService) { }

  makeCall(queryBody): string {

    const from = '+18592983029';
    const xmlUrl = 'http://pegando-fogo-3o3l.localhost.run';

    let twilio = new Twilio('ACc8771dd2f216225085878a51da25b9bf', '674cc7f8c7ea352de6005420aa086443');

    // this.httpClient.get(`http://api.openweathermap.org/data/2.5/weather?lat=${queryBody.}&lon=-49.37944&appid=f48d93cf79caeb4cdf990386aa8df5f3&units=metric`)

    let umidade = queryBody.umidade || '13';
    let vento = queryBody.vento || '6';
    let lat = queryBody.latLng.split(' ')[0];
    let lng = queryBody.latLng.split(' ')[1];
    let usuarios = queryBody.usuarios || '2';
    let confidence = queryBody.confidence || '93';
    let to = queryBody.tel || '+5518991005519';

    let xml = jsonxml({
      Response: [
        {
          name: 'Say',
          attrs: {
            voice: 'alice',
            language: 'pt-BR'
          },
          text: `Olá. Detectamos um foco de incêndio na coordenada latitude ${lat} longitude ${lng}`
        }, {
          name: 'Pause',
          attrs: {
            length: '1'
          }
        }, {
          name: 'Say',
          attrs: {
            voice: 'alice',
            language: 'pt-BR'
          },
          text: `Este foco foi confirmado por ${usuarios} usuarios e foi aprovado na validação com confiança de ${confidence}%`
        }, {
          name: 'Pause',
          attrs: {
            length: '1'
          }
        }, {
          name: 'Say',
          attrs: {
            voice: 'alice',
            language: 'pt-BR'
          },
          text: `A velocidade do vento atual é de ${vento} metros por segundo, com umidade relativa da região de ${umidade}%`
        }
      ]
    });

    let filename = uuid.v4() + '.xml';

    fs.writeFileSync(`/var/www/html/${filename}`, xml);

    twilio.calls.create({
      from,
      to,
      method: 'GET',
      url: xmlUrl + `/${filename}`,
      record: true
    });

    return xml;
  }
}
