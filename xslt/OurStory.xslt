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
<xsl:variable name="ourStoryPromos" select="umbraco.library:GetXmlNodeById($currentPage/ourStoryPromos)"/>
	
<xsl:template match="/">

<!-- start writing XSLT -->
	
	<section id="our_story">
		<div class="container">
			<h3><xsl:value-of select="$currentPage/documentTitle"/></h3>
			
			<xsl:for-each select="$ourStoryPromos/descendant::*[@isDoc]">
				<xsl:choose>
					<xsl:when test="position() mod 2 =1">
						<div class="line">&nbsp;</div>
						<div class="row our_story_list_topspace">

							<div class="col-sm-6">
								<h4>
									<xsl:value-of select="./ourStoryTitle"/>
								</h4>
								<xsl:value-of select="./ourStoryDescription" disable-output-escaping="yes"/>
							</div>
							<div class="col-sm-6 our-story-img text-center">
								<xsl:variable name="mediaId" select="./ourStoryImage" />
								<xsl:if test="$mediaId != ''">
									<!--<img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
										 title="{$currentPage/@nodeName}"
										 alt="{$currentPage/@nodeName}"											
										 width="{umbraco.library:GetMedia($mediaId, 'false')/umbracoWidth}"
										 height="{umbraco.library:GetMedia($mediaId, 'false')/umbracoHeight}">
									</img>-->
	                <img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
										 title="{$currentPage/@nodeName}"
										 alt="{$currentPage/@nodeName}"	
                       class=" img_size"
										 style="" >
									</img>        
                
                
								</xsl:if>

							</div> 
						</div>
					</xsl:when>
					<xsl:otherwise>
						<div class="line">&nbsp;</div>
						<div class="row our_story_list_topspace">
							<div class="col-sm-6 text-center our_story_img_left erl_desc">

								<xsl:variable name="mediaId" select="./ourStoryImage" />
								<xsl:if test="$mediaId != ''">
									<img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
										 title="{$currentPage/@nodeName}"
										 class="mobile_ourstory img_size"
										 alt="{$currentPage/@nodeName}"											
										 
                         style="">
									</img>
								</xsl:if>
							</div> 
							<div class="col-sm-6 ">
								<div class="our_story_img_right">
									<h4>
										<xsl:value-of select="./ourStoryTitle"/>
									</h4>
									<xsl:value-of select="./ourStoryDescription" disable-output-escaping="yes"/>
								</div>
							</div>              
						</div>
            <div class="row">
            <div class="col-sm-6">&nbsp;</div>
              <div class="col-sm-6 our-story-img text-center ltr_img">
                <xsl:variable name="mediaId" select="./ourStoryImage" />
                <xsl:if test="$mediaId != ''">
                  <img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
										 title="{$currentPage/@nodeName}"
										 class="mobile_ourstory img_size"
										 alt="{$currentPage/@nodeName}"
										 
                         style="">
                  </img>
                </xsl:if>
              </div>
            </div>
            
            
          </xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>	 

		</div>
	</section>
	 

</xsl:template>

</xsl:stylesheet>