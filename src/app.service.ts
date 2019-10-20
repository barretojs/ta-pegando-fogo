import { Injectable, HttpService } from '@nestjs/common';

import { Twilio } from 'twilio';
import * as jsonxml from 'jsontoxml';
import * as fs from 'fs';
import * as uuid from 'uuid';

@Injectable()
export class AppService {

  constructor(private readonly httpClient: HttpService) { }

  makeCall(queryBody): string {

    const from = '';
    const xmlUrl = '';

    let twilio = new Twilio('', '');

    let umidade = queryBody.umidade || '13';
    let vento = queryBody.vento || '6';
    let lat = queryBody.latLng.split(' ')[0];
    let lng = queryBody.latLng.split(' ')[1];
    let usuarios = queryBody.usuarios || '2';
    let confidence = queryBody.confidence || '93';
    let to = queryBody.tel || '';

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
