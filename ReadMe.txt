RESTful Web service

GET		/api/product/1		client sends id
GET		/api/product?id=1	client sends id		
GET		/api/product		client sends nothing => list of products
GET		/api/prop=value		filtering
POST	/api/product		client sends product
PUT		/api/product		modified product to be updated
DELETE	/api/product/1		product to be deleted
DELETE	/api/product/id=1	product to be deleted

Web API 2.0 Action Method Result
	- HttpRequestMessage describes the request to be handled
	- HttpResponseMessage describes the response to be returned
	- IIS is responsible for creating HttpResponseMessage object to represent the request and turning HttpRequestMessage into HTTP response and sending it to client
	- FLOW: Client => Request => Hosting (IIS creates HttpRequestMessage) => Controller (HttpResponseMessage) => Action
	
	A. HttpRequestMessage properties
		Content - Returns Http content
		Headers - HttpRequestHeaders object
		Methods - Get/Post/Delete etc
		Properties - dictionary contains objects provided by host
		RequestUri - returns URL requested by the client
		Version - version of HTTP that was used to make request

	B. HttpResponseMessage - result methods
		- IHttpActionResult is equivalent to AcitonResult class in Mvc framework => Task<HttpResponseMessage>
		BadRequest() - status code 400
		BadRequest(message) - 400 with message
		BadRequest(modelstate) - 400 with validation messages
		Conflict - 409
		Content(status, data)/(url, data)
		InternalServerError() - 500 exception
		NotFound() - 404 creates not found result
		Ok() - 200 OkResult
		Ok(data) 
		Redirect(target) - 302 creates redirect result (Uri)
		RedirectToRoute(name, props) - 302 creates a redirectToRouteResult which generates URL from routing configuration

		- Creating custom Action Result by implementing IHttpActionResult => Task<HttpResponseMessage> ExecuteAsync(CancellationToken)



Transport Security

Authentication - loggin in 
Authorization - what access leve a client gets

HTTP => no built in security
SSL/TLS => tunnel uprotected HTTP over secure channel: HTTPS = HTTP over SSL
	- server authentication
	- integrity protection
	- replay protection
	- confidentiality

SSL
 X509Certificate 
	- file that contains info about owner of the certificte and who issued certificate (how long it's valid, public key)
	- issued to a web server (ex. Public key: Issue to field: Amazon.com => / Issuer: Verisign, Godaddy etc)
	- Public Key: meaning I can send server encripted data and that server can decrypt that data with corresponding private key (securely stored on the server)
 SSL Handshake
	1. browser send request to the server (amazon.com)
	2. amazon sends certificate (file) back to browser
	3. browser inspects contents of the certificate (name in "issue to field" matches DNS name in the browser address bar) and checks issuer, expiration date etc
	4. browser => generate session key and encrypt with public key found in the certificate sent from the server for the rest of connection
	5. communication fron now on is encrypted
 Where do you get certificates from?
	- Buy a certificate: Verisign, Godaddy
	- Corporate PKI (Windows certiricate services)
	- Create yourself (makecert.exe, openSSL)

1. Creating certificate (Run as administrator)
	Root Certificate
	makecert -r -n "CN=DevRoot" -pe -sv DevRoot.pvk -a sha1 -len 2048 -b 01/21/2010 -e 01/21/2030 -cy authority "DevRoot.cer"
	pvk2pfx -pvk DevRoot.pvk -spc DevRoot.cer -pfx DevRoot.pfx

	SSL Certificate (Using above root cert create new certificate)
	makecert -iv DevRoot.pvk -ic DevRoot.cer -n "CN=web.local" -pe -sv web.local.pvk -a sha1 -len 2048 -b 01/21/2010 -e 01/21/2030 -sky exchange web.local.cer -eku 1.3.6.1.5.5.7.3.1
	pvk2pfx -pvk web.local.pvk -spc web.local.cer -pfx web.local.pfx

	SSL Client Certificate -- // different by -eku digit number
	makecert -iv DevRoot.pvk -ic DevRoot.cer -n "CN=clientssl.local" -pe -sv clientssl.local.pvk -a sha1 -len 2048 -b 01/21/2010 -e 01/21/2030 -sky exchange clientssl.local.cer -eku 1.3.6.1.5.5.7.3.2
	pvk2pfx -pvk clientssl.local.pvk -spc clientssl.local.cer -pfx clientssl.local.pfx


	Another Certificate
	makecert -iv DevRoot.pvk -ic DevRoot.cer -n "CN=as.local" -pe -sv as.local.pvk -a sha1 -len 2048 -b 01/21/2010 -e 01/21/2030 -sky exchange as.local.cer -eku 1.3.6.1.5.5.7.3.1
	pvk2pfx -pvk as.local.pvk -spc as.local.cer -pfx as.local.pfx


	Make ROOT certificate trusted (MMC)
		Personal Folder - where certificates that have private key are installed
		Trusted Root Cert Authorities - where Root certificates are installed that Windows trusts by default

		a. Import ROOT certiciate we created to Trusted Root Certification Authority - DevRoot.cert
		b. Import web.local certificate we created to Personal\Certficates - web.local.pfx (this contains public key)

2. X509Certificate2
	X509CertificateValidator class - used for validating certificates

3. Authentication and Authorization (Ver 2.0)
	- OWIN (Host) => Web API [ Message hander => Authentication Filter => Authorization Filter ] --- control flow
	- Host => OWIN - host/framework (Microsoft built it's own version called Katana on top of Owin)
		a. Owin is like a hosting adapter
		b. allows you to use Google, Facebook authentication etc

	Pipeline: IIS => ASP.net (OWIN bridge) => OWIN => Web API (+ Owin adapter)
	
	Why OWIN => IIS opens TCP channel and send data from/to app. Microsoft wants to decouple that process, hence OWIN/Katana solution
			=> use hosting .net envirenment which is independent of actual HOST (IIS)

4. WEB.Api Identiy
	[Authorize], [AllowAnonymous] at Acion level - used when [Authorize] filter is used at Controller level. This will allow anonymous access and overwrite controller's [Authorize]
	[Authorize(Role="Foo")] - must be in role Foo

	Accessing client identity:
		a. Previous versions => Thread.CurrentPrincipal
		b. Ver 2.0 => ApiController.User (HttpRequestMessage) / RequestContext

	Solution to set Identity setting/getting (best practice is to do it in OWIN Middleware)
	A. Adding custom module (in web.config)		Message Handlers => Not recommended to use for authentication	// IIS
	B. Adding Custom Middleware => OWIN adding additional custom middleware components in the processing pipeline	// OWIN Middleware
	C. TestAuthenticationFilterAttribute	// Authentication
	D. TestAuthorizationFilterAttribute		// Authorization

5. Owin HOST (Running Web Api NOT on IIS but on OWIN host) => Helios from Microsoft (download nuget package, change in properties)

6. Windows Authentication / Basic Authentication / SSL client certification
	A. Windows Authentication - Intranet scenario
		- all parties belong to Active Directory, no node, just configuration - Client machine belongs to domain (local)
		- log on to your machine, those credentials are used for authontication in your domain
		- Web.Config changes + properties set "Anonymous Authentication" to false
		Claim Transformation => grabbing Windows identity info and getting more stuff from DB and adding/changing additional claims in OWIN middleware
		Creating client to call Web Api with Windows Credentials
			HttpClientHandler, HttpClient

	B. Basic Authentication - Old way, not used anymore
		- Client must store the secret login info or obtain it from the user (on every request)
		- Server has to validate it on every request (time cost due to server computation/decryption etc)

	C. X509 Client Certificates
		- Popular option for "high security" scenarios
		- Can be combined with other authentication methods (ie. Basic Authentication)
		How do you access client certificate from request? => from RequestContext => request.GetRequestContext().ClientCertificate;

		1. Create clientssl certificate and import to Current User certificate store (Personal)

7. Javascript/Browser based Authentication
	- Same Origin Policy - security feature that makes sure that only browser running on the same domain can access server on the same domain
		- must have same Host and same protocol (either Http or Https)
		CSRF - Cross-Site Request Forgery (when a browswer gets a cookie from the server. You open another tab but this time open a website that does
			not make requests from the same domain BUT You already have a cookie stored in the browser, so hacker website can call/post()/get() your original rest api stuff with that cookie)
		MVC4 - this is solved in MVC4 by using [ValidateHttpAntiForgeryToken]
			- this works when MVC generates a hidden field with anti-forgery token value, when posted back to server, server verifies this - But HOW do you do this in Web Api? there is no forms
	HOW? - Web API ver. 1
		WEB Api - When you call web api you, client sends a cookie + header (usually from ajax request), 
			when calls comes back server takes the cookie + header and runs math on both values/validation
		- hacker may only gain access to the cookie BUT NOT the form ticket value that originated from original client (ajax) call

		Server => render page + forgery cookie => post back: cookie + hidden field => web api call: cookie + header

	WEB API v.2 CSRF - no more cookies are used
		- Token based Authentication - no longer same domain needed for client and server
			config.SupressDefaultHostAuthentication() - disable any host authentication
			config.Filters.Add(new HostAuthenticationFilter(OAthDefaults.AuthenticationType)) - look for client token only
			
		Once logged in .ASPXAUTH cookie is sent back and forth between client and server
		- Add Token to ajax call
		Process: 
			a. From Web form grab AntiForgeryToken (in hidden field) and stick it to header request of ajax call
			b. Web Api is decorated with [ValidateAntiForgeryToken] that will validate token sent from the client

8. Token Based Authentication
	Modern applications: Users that will use Web APIs / Clients / Web APIs
	OAUTH2 (Authorization framework) => an open protocol to allow secure authorization in simple and standard method from web, modbile, and desktop applications
		- enables a third party applications to obtain limited access to an HTTP service, either on behalf of the resource owner or by allowing
		the third party application to obtain access on its own behalf.

	OAuth2 Approach: Client (HTML, Android, .NET, Java etc) => Authorization Server => Web APIs (applications)
		Scopes: (read, write, delete, search) - access level (ex: desktop should have full access BUT mobile only read etc)
		Process: Bob (client_id = 1, scope = search/read) => Authorization server => access token (returned to client from server) => Client now uses access token to act on behalf of the user
		Access Token {
				iis: Where the token is coming from - issuer
				aud: application (audience)
				exp: how long it's valid
				sub: who is the user
				client_id: who is the client
				scope: access leve
		}


9. Authorization (WebApi2_Authentication project) [Authorize(Roles="SomeRole")] => [CustomAuthorizationAttribute]
	- User's identity: claims from token (from DB or profile data)
	- Client: (identity, client type, scope)

	What's the best place to do Authorization?
		- OWIN Middleware
		- global authorization filter
		- controller authorization filter
		- action authorization filter

	Adding filter globally in WebApiConfig.cs
		config.Filters.Add(new CustomAuthorizationAttribute());

	ClaimsAuthorizationManager - write a class that derives from this (this controls access level) => AuthorizationManager
		then specify ClaimsAuthorizationManager in web.config or in Startup.cs






	
	