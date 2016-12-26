<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#x00A0;">
]>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxml="urn:schemas-microsoft-com:xslt"
	xmlns:umbraco.library="urn:umbraco.library" xmlns:Examine="urn:Examine"
	exclude-result-prefixes="msxml umbraco.library Examine ">


  <xsl:output method="xml" omit-xml-declaration="yes"/>

  <xsl:param name="currentPage"/>

  <xsl:variable name="HomeRow3Promo" select="umbraco.library:GetXmlNodeById($currentPage/row3Promo)"/>

  <xsl:template match="/">

    <!-- start writing XSLT -->

    <section id="Scottybons-stand">
      <div class="container">
        <xsl:for-each select="$HomeRow3Promo/descendant::*[@isDoc]">
          <xsl:if test="./promoTitle != ''">
            <div class="col-sm-4 Scottybons-standheading">
              <h1>
                <xsl:value-of select="./promoTitle"/>
              </h1>
            </div>

            <div class="col-sm-8 Scottybons-stand-right-border">
              <xsl:value-of select="./promoContent" disable-output-escaping="yes"/>
            </div>
          </xsl:if>
        </xsl:for-each>
      </div>
    </section>

  </xsl:template>

</xsl:stylesheet>