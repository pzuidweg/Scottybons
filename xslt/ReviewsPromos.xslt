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
<xsl:variable name="reviewPromos" select="umbraco.library:GetXmlNodeById($currentPage/reviewPromos)"/>
	
<xsl:template match="/">

<!-- start writing XSLT -->
	
	<section id="reviews">
		<div class="container">
			<h3>
				<xsl:value-of select="$currentPage/documentTitle"/>
			</h3>
			
			<xsl:for-each select="$reviewPromos/descendant::*[@isDoc]">
				<xsl:sort select="styleTipsDate"  order="descending" />
				
						<!-- <div class="line">&nbsp;</div> -->
						<div class="row our_story_list_topspace">

              <div class="col-sm-12 bottom-line">
                <div class="row">
                <div class="col-sm-6">
                  <div class="row">
                    <h4>
                      <xsl:value-of select="./reviewTitle"/>
                    </h4>
                  </div>
                </div>
                <div class="col-sm-6 text-right">
                  <div class="row">
                    <h4>
                      <xsl:value-of select="umbraco.library:FormatDateTime(./reviewDate, 'dd-MM-yyyy')"/>
                    </h4>
                  </div>
                </div>
                </div>
              </div>

                <div class="col-sm-12">
                  <div class="row">

                    <xsl:value-of select="./reviewDescription" disable-output-escaping="yes"/>
                  </div>
							</div>
							
						</div>
					
			</xsl:for-each>	 

		</div>
	</section>
	 

</xsl:template>

</xsl:stylesheet>