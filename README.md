Version 1.0
Documentation PDF: 
SDK: .NET Core 6.0

url swagger: https://localhost:7057/swagger/index.html
Developed By: Iván Darío Vergara Fuentes


*******************************************************************************************************************************************
Método: Token
Url: https://localhost:7057/api/v1/Token
Descripcion: Genera token utilizado para la autenticación de los método, TTL 20 min

usuario token:
{
  "userName": "api_dragonBall",
  "password": "Nub3V0l4d0r4"
}
____________________________________________________________________________________________________________________________________________

Método: ListarBatallasDragonBall
Url: https://localhost:7057/api/v1/ProgramarBatallas

Descripcion: Sirve para consultar y listar batallas de personajes DragonBall
entrada: numeroParticipantes, Número entero que corresponde al número de luchadores que participarán en el evento.
Solo se admite numero par positivo y menor o igual que 16.

URL API Cliente:  https://dragonball-api.com/api/characters?page=1&limit={numeroParticipantes}
___________________________________________________________________________________________________________________________________________
