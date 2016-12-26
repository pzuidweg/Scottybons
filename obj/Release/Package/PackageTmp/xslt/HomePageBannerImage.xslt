<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>
<xsl:stylesheet 
	version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:msxml="urn:schemas-microsoft-com:xslt"
	xmlns:umbraco.library="urn:umbraco.library" xmlns:Examine="urn:Examine" 
	exclude-result-prefixes="msxml umbraco.library Examine ">


<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:param name="currentPage"/>

<xsl:template match="/">
	
	<xsl:variable name="mediaId" select="($currentPage/bannerImage)" />
	
	<xsl:if test="$mediaId != ''">
		
	<div id="slider_continer" style="background:#ccc url('{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}');">
		<div class="container">
			<div class="row">
				<div class="col-md-12" >
					<div class="col-md-6">
						<div class="intro_title_booking" >
							<div class="line">&nbsp;</div>
							<xsl:value-of select="$currentPage/bannerImageContent" disable-output-escaping="yes"/>
							<div class="line">
								&nbsp;</div> 
						</div>
						<div class="row text-center"> <a href="{umbraco.library:NiceUrl($currentPage/redirectionLink)}" class="animated fadeInUp button_intro outline">
								<xsl:value-of select="$currentPage/anchorText" />
								</a> </div>
						</div>
						<div class="col-md-6">
							&nbsp;
						</div>
					</div>
				</div>
				<!-- End row -->
			</div>
		</div>
		
</xsl:if>
	
	
</xsl:template>

</xsl:stylesheet>